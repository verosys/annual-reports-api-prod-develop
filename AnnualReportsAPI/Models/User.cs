using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace AnnualReportsAPI.Models
{
  public class User
  {
    public ObjectId _id { get; set; }
    public string ClientId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string CompanyName { get; set; }
    public string Contact { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public string Phone { get; set; }
    public string[] Roles { get; set; }
    public Package Package { get; set; }
    [BsonIgnoreIfNull]
    public string PasswordRecoverToken { get; set; }
    [BsonIgnoreIfNull]
    public DateTime PasswordRecoverTokenExpireUTC { get; set; }

    [BsonIgnoreIfNull]
    public IEnumerable<Client> Clients { get; set; }

    public User()
    {
      Roles = new List<string>().ToArray();
    }
  }

  public class UserDTO
  {
    private IEnumerable<Client> _clients;

    public string Email { get; set; }
    public string CompanyName { get; set; }
    public string Contact { get; set; }
    public Package Package { get; set; }
    public int ClientsCount { get
      {
        if (this._clients == null)
          return 0;

        return this._clients.Count();
      }
    }

    public UserDTO(IEnumerable<Client> clients)
    {
      this._clients = clients;
    }
  }

  public class Package
  {
    public string Name { get; set; }
    public int ValidForYear { get; set; }
  }

  public class RegisterDetails
  {
    [Required]
    public string Email { get; set; }
    [MinLength(7)]
    public string Password { get; set; }
    public string CompanyName { get; set; }
    public string Contact { get; set; }
    public string Phone { get; set; }

  }
}
