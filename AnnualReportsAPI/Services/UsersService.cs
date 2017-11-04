using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using AnnualReportsAPI.Models;
using MongoDB.Bson.Serialization;
using AnnualReportsAPI.Options;
using Microsoft.Extensions.Options;
using AnnualReportsAPI.Utils;
using AnnualReportsAPI.Exceptions;
using System.Globalization;
using System.Net;
using Microsoft.Extensions.Logging;
using MongoDB.Driver.Core.Events;
using Microsoft.AspNetCore.Hosting;

namespace AnnualReportsAPI.Services
{
  public class UsersService
  {
    private MongoClient _client;
    private IMongoDatabase _db;
    private IMongoCollection<User> _usersCollection;
    private MongoDBOptions _mongoDBOptions;
    private SendGridOptions _sendGridOptions;
    private EmailService _emailService;
    private ProjectGeneralsOptions _projectGenerals;
    private ILogger _logger;
    private IHostingEnvironment _env;

    public UsersService(IHostingEnvironment env,
                          IOptions<MongoDBOptions> options,
                            IOptions<SendGridOptions> sendGridOptions,
                              IOptions<ProjectGeneralsOptions> projectGeneralsOptions,
                                EmailService emailService,
                                  ILoggerFactory loggerFactory)
    {
      _env = env;
      _mongoDBOptions = options.Value;
      _sendGridOptions = sendGridOptions.Value;
      _projectGenerals = projectGeneralsOptions.Value;
      _logger = loggerFactory.CreateLogger<UsersService>();
      _emailService = emailService;

      //if (this._env.IsDevelopment())
      //{
      //  MongoUrl murl = new MongoUrl(_mongoDBOptions.Url);
      //  MongoClientSettings mcs = MongoClientSettings.FromUrl(murl);
      //  mcs.ClusterConfigurator = c =>
      //  {
      //    c.Subscribe<CommandStartedEvent>(e =>
      //    {
      //      this._logger.LogInformation($"{e.CommandName} - {e.Command.ToJson()}");
      //    });
      //    //c.Subscribe<CommandSucceededEvent>(this.CommandExecutedHandler);
      //  };
      //  _client = new MongoClient(mcs);
      //}
      _client = new MongoClient(_mongoDBOptions.Url);

      _db = _client.GetDatabase(_mongoDBOptions.DbName);
      _usersCollection = _db.GetCollection<User>("users");

      //Create index not supported in MongoDB API at the moment
      //var indexOptions = new CreateIndexOptions() { Unique = true};
      //_usersCollection.Indexes.CreateOne(new IndexKeysDefinitionBuilder<User>().Ascending("Email"), indexOptions);
    }

    public User Get(string id)
    {
      User user = this._usersCollection.Find(new BsonDocument { { "_id", new ObjectId(id) } }).FirstAsync().Result;
      LogCosmosDBRequestCharge();

      return user;
    }

    public async Task<PagedResult<IEnumerable<UserDTO>>> SearchUsers(string startWith = null, int? page = 1, int? pageSize = 7)
    {
      PagedResult<IEnumerable<UserDTO>> finalResult = new PagedResult<IEnumerable<UserDTO>>()
      {
        Pagination = new PagingDetails()
        {
          Count = 0,
          Page = 1,
          PerPage = pageSize.Value
        }
      };

      var filter = Builders<User>.Filter.Regex("Email", new BsonRegularExpression($"^{startWith}", "i"));
      var users = await this._usersCollection.Find(filter)
                            .Project(u => new UserDTO(u.Clients)
                            {
                              Email = u.Email,
                              CompanyName = u.CompanyName,
                              Contact = u.Contact,
                              Package = u.Package
                            })
                            .ToListAsync();

      LogCosmosDBRequestCharge();

      int count = users.Count();
      var usersCurrentPage = users.Skip<UserDTO>((page.Value - 1) * pageSize.Value).Take<UserDTO>(pageSize.Value);

      if (usersCurrentPage == null)
        return finalResult;

      finalResult.Data = usersCurrentPage;
      finalResult.Pagination.Page = page.Value;
      finalResult.Pagination.PerPage = pageSize.Value;
      finalResult.Pagination.Count = count;


      return finalResult;
    }

    public async Task<PagedResult<List<ClientDTO>>> ListClients(string clientId, string startWith = null, int? page = 1, int? pageSize = 7)
    {
      PagedResult<List<ClientDTO>> finalResult = new PagedResult<List<ClientDTO>>()
      {
        Pagination = new PagingDetails()
        {
          Count = 0,
          Page = 1,
          PerPage = pageSize.Value
        }
      };

      var filter = Builders<User>.Filter.Eq("ClientId", clientId);
      var clients = await this._usersCollection.Find(filter).Project(user => user.Clients).FirstOrDefaultAsync();

      LogCosmosDBRequestCharge();

      if (!String.IsNullOrEmpty(startWith))
      {
        clients = clients.Where(c => !String.IsNullOrEmpty(c.Name) && c.Name.StartsWith(startWith, StringComparison.OrdinalIgnoreCase));
      }

      int count = clients.Count();

      clients = clients.Skip<Client>((page.Value - 1) * pageSize.Value).Take<Client>(pageSize.Value);

      if (clients == null)
        return finalResult;

      List<ClientDTO> finalList = new List<ClientDTO>();

      foreach (Client c in clients)
      {
        ClientDTO cDTO = new ClientDTO()
        {
          City = c.City,
          County = c.County,
          Name = c.Name,
          Oib = c.Oib,
          Place = c.Place,
          GfiUploads = c.GfiUploads
        };
        finalList.Add(cDTO);
      }

      finalResult.Data = finalList;
      finalResult.Pagination.Page = page.Value;
      finalResult.Pagination.PerPage = pageSize.Value;
      finalResult.Pagination.Count = count;

      return finalResult;
    }

    public async Task<ClientDTO> GetClient(string clientId, string oib)
    {
      var filter = Builders<User>.Filter.Eq("ClientId", clientId)
                   & Builders<User>.Filter.ElemMatch<Client>($"Clients", Builders<Client>.Filter.Eq("Oib", oib));
      var client = await this._usersCollection.Find(filter).Project(user => user.Clients.Where(c => c.Oib == oib).FirstOrDefault()).FirstOrDefaultAsync();
      //var client = await this._usersCollection.Find(filter).FirstOrDefaultAsync();

      LogCosmosDBRequestCharge();

      if (client == null)
        return null;

      ClientDTO cDTO = new ClientDTO()
      {
        City = client.City,
        County = client.County,
        Name = client.Name,
        Oib = client.Oib,
        Place = client.Place,
        GfiUploads = client.GfiUploads
      };

      return cDTO;
    }

    public GfiUpload GetGfiInfo(string clientId, string oib, int year)
    {
      //var filter = Builders<User>.Filter.Eq("ClientId", clientId) & Builders<User>.Filter.Eq($"Clients.GfiUploads.Year", year);
      var filter = Builders<User>.Filter.Eq("ClientId", clientId);
      //& Builders<User>.Filter.ElemMatch("Clients.GfiUploads", Builders<GfiUpload>.Filter.Eq("Year", year));

      var gfiUpload = this._usersCollection
                          .Find(filter)
                          .Project(user => user.Clients.Where(c => c.Oib == oib))
                          .FirstOrDefault()
                          .Select(c => c.GfiUploads
                                        .Where(g => g.Year == year)
                                        .FirstOrDefault()
                          )
                          .FirstOrDefault();

      LogCosmosDBRequestCharge();

      return gfiUpload;
    }

    public int CountGfiPerYear(string clientId, int year)
    {
      var filter = Builders<User>.Filter.Eq("ClientId", clientId)
                    & Builders<User>.Filter.ElemMatch<Client>("Clients", Builders<Client>.Filter.ElemMatch<GfiUpload>("GfiUploads", Builders<GfiUpload>.Filter.Eq("Year", year)));
      var gfiUpload = this._usersCollection
                          .Find(filter)
                          .Project(user => user.Clients)
                          .FirstOrDefault();

      LogCosmosDBRequestCharge();

      return gfiUpload.Count();
    }

    public async Task<User> FindByEmailPassword(string email, string password)
    {
      var filter = Builders<User>.Filter.Eq("Email", email);
      User user = await this._usersCollection
                      .Find<User>(filter)
                      .FirstOrDefaultAsync<User>();

      LogCosmosDBRequestCharge();

      if (user == null)
        return null;

      bool passMatch = SecurityUtils.CheckMatch(user.Password, password);
      if (passMatch != true)
        return null;
      else
        return user;
    }

    public async Task<UserDTO> FindByEmail(string email)
    {
      var filter = Builders<User>.Filter.Eq("Email", email);
      var user = await this._usersCollection
                           .Find<User>(filter)
                           .Project(u => new UserDTO(u.Clients)
                           {
                             Email = u.Email,
                             CompanyName = u.CompanyName,
                             Contact = u.Contact,
                             Package = u.Package
                           })
                           .FirstOrDefaultAsync<UserDTO>();

      LogCosmosDBRequestCharge();

      return user;
    }

    public async Task<User> CreateNew(User user)
    {
      await this._usersCollection.InsertOneAsync(user);

      LogCosmosDBRequestCharge();

      return Get(user._id.ToString());
    }

    public async Task<Client> AddGfiToClient(string clientId, Client client, GfiUpload gfi)
    {
      var filter = Builders<User>.Filter.Eq("ClientId", clientId);
      var clients = await this._usersCollection.Find(filter).Project(user => user.Clients).FirstOrDefaultAsync();
      LogCosmosDBRequestCharge();

      List<Client> clientsTmp = clients != null ? clients.ToList<Client>() : new List<Client>();
      int index = clientsTmp.FindIndex(c => c.Oib == client.Oib);

      if(index != -1)
      {
        clientsTmp[index].Update(client.Name, client.Place, client.County, client.City);
        clientsTmp[index].AddGfiUpload(gfi);
      }
      else
      {
        client.AddGfiUpload(gfi);
        clientsTmp.Add(client);
      }

      var update = Builders<User>.Update.Set($"Clients", clientsTmp.AsEnumerable<Client>());
      await this._usersCollection.UpdateOneAsync(filter, update);
      LogCosmosDBRequestCharge();

      return client;
    }

    public async Task<User> ReplaceUser(string id, User user)
    {
      user._id = new ObjectId(id);

      var filter = Builders<User>.Filter.Eq(s => s._id, user._id);
      await this._usersCollection.ReplaceOneAsync(filter, user);

      LogCosmosDBRequestCharge();

      return this.Get(id);
    }

    public async Task<string> CreateRecoverToken(string email)
    {
      string recoverToken = SecurityUtils.GeneratePasswordRecoveryTokenString(41, false);
      var filter = Builders<User>.Filter.Eq("Email", email);
      var update = Builders<User>.Update.Set("PasswordRecoverToken", recoverToken)
                                        .Set("PasswordRecoverTokenExpireUTC", DateTime.UtcNow.AddHours(1));
      var result = await this._usersCollection.UpdateOneAsync(filter, update);

      LogCosmosDBRequestCharge();

      if (result.IsAcknowledged && result.MatchedCount == 1)
      {
        DateTime thisDate = DateTime.Now;
        DateTimeFormatInfo fmt = (new CultureInfo("hr-HR")).DateTimeFormat;

        string plainTextContent = $"Sigurnosni kod za promjenu lozinke je: {recoverToken}";
        string htmlTemplate = @"<div>
                                <p>Za promjenu lozinke kliknite na link: <a href='{{resetPasswordlink}}'>promjena lozinke</a>.</p>
                                <p style = 'color:#aaa;font-size:12px;' > Ukoliko niste zatražili promjenu lozinke, slobodno ignorirajte ovaj mail.
                                <br /> Ova poruka generirana je: {{resetPasswordDateTime}}. Link za promjenu lozinke vrijedi 60 minuta. 
                                Ukoliko ne uspijete promijeniti lozinku u tom periodu, zatražite novi sigurnosni kod.</ p >
                                </div >";
        string htmlContent = htmlTemplate.Replace("{{resetPasswordlink}}", $"{this._projectGenerals.PasswordResetAddress}{WebUtility.UrlEncode(recoverToken)}");
        htmlContent = htmlContent.Replace("{{resetPasswordDateTime}}", thisDate.ToString("F", fmt));

        await this._emailService.Send(email, "Promjena lozinke", plainTextContent, htmlContent);

        return recoverToken;
      }
      else
      {
        return null;// throw new ServiceOperationException("Unknown email.");
      }
    }

    public async Task ResetPassword(string recoveryToken, string password)
    {
      string hash = SecurityUtils.EncryptPassword(password);

      var filter = Builders<User>.Filter.Eq("PasswordRecoverToken", recoveryToken) &
                   Builders<User>.Filter.Gt<DateTime>("PasswordRecoverTokenExpireUTC", DateTime.UtcNow);

      var update = Builders<User>.Update.Set("Password", hash)
                                        .Unset("PasswordRecoverToken")
                                        .Unset("PasswordRecoverTokenExpireUTC");

      var result = await this._usersCollection.UpdateOneAsync(filter, update);

      LogCosmosDBRequestCharge();

      if (result.IsAcknowledged && result.MatchedCount == 1)
      {
        return;
      }
      else
        throw new ServiceOperationException("Password could not be reset.");
    }

    private void LogCosmosDBRequestCharge()
    {
      if (this._env.IsDevelopment())
      {
        var result = this._db.RunCommand<BsonDocument>(new BsonDocument { { "getLastRequestStatistics", 1 } });
        this._logger.LogInformation($"{result}");
      }
    }

    //private void CommandExecutedHandler(CommandSucceededEvent e)
    //{
    //  var result = this._db.RunCommand<BsonDocument>(new BsonDocument { { "getLastRequestStatistics", 1 } });
    //  this._logger.LogInformation($"{result}");
    //}
  }
}
