using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using AnnualReportsAPI.ErrorResponses;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using AnnualReportsAPI.Options;
using Microsoft.Extensions.Options;
using AnnualReportsAPI.Services;
using AnnualReportsAPI.Models;
using AnnualReportsAPI.Extensions;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace AnnualReportsAPI.Controllers
{
  [Route("api/v.1/[controller]")]
  [CustomExceptionFilter]
  [Authorize]
  public class ClientsController : Controller
  {
    IHostingEnvironment _env;
    private UsersService _usersService;
    private GfiUploadOptions _gfiUploadOptions;
    private FileStorageService _fileStorageService;
    private ILogger _logger;

    public ClientsController(IHostingEnvironment env,
                              UsersService usersService,
                                IOptions<GfiUploadOptions> gfiUploadOptions,
                                  FileStorageService fileStorageService,
                                    ILoggerFactory loggerFactory)
    {
      _env = env;
      _usersService = usersService;
      _gfiUploadOptions = gfiUploadOptions.Value;
      _fileStorageService = fileStorageService;
      _logger = loggerFactory.CreateLogger<ClientsController>();
    }

    [HttpGet()]
    public async Task<IActionResult> List(string q = null, int? page = 1, int? pageSize = 7)
    {
      string clientId = User.GetClientId();
      return Ok(await this._usersService.ListClients(clientId, q, page, pageSize));
    }

    [HttpGet("{oib}")]
    public async Task<IActionResult> Get(string oib)
    {
      string clientId = User.GetClientId();
      return Ok(await this._usersService.GetClient(clientId, oib));
    }

    [HttpGet("{oib}/gfi/{year}")]
    public IActionResult GetGfiInfo(string oib, int year)
    {
      string clientId = User.GetClientId();
      //var i = this._usersService.CountGfiPerYear(clientId, year);
      //this._logger.LogInformation($"Clients for year {year.ToString()}: {i.ToString()}");
      return Ok(this._usersService.GetGfiInfo(clientId, oib, year));
    }

    [HttpGet("{oib}/gfi/year/{year}/doctype/{documentType}/outputtype/{outputType}")]
    public async Task<IActionResult> DownloadGfiDocument(string oib, int year, string documentType, string outputType)
    {
      string clientId = User.GetClientId();

      string pathToFile = System.IO.Path.Combine(_env.ContentRootPath, this._gfiUploadOptions.UploadFolder, "6984341_2.pdf");
      this._logger.LogInformation(pathToFile);
      string contentType = String.Empty;
      string fileExtension = String.Empty;

      switch (outputType)
      {
        case "pdf":
          contentType = "application/pdf";
          fileExtension = "pdf";
          break;

        case "docx":
          contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
          fileExtension = "docx";
          break;
      }
      string filename = $"GFI-{new DateTime().ToString()}.{fileExtension}";

      var contentDisposition = new ContentDispositionHeaderValue("attachment");
      contentDisposition.FileName = filename;
      Response.Headers[HeaderNames.ContentDisposition] = contentDisposition.ToString();
      //var stream = new System.IO.FileStream(pathToFile, System.IO.FileMode.Open);
      var stream = await this._fileStorageService.GetFile(clientId, "6984341_2.pdf");
      
      return File(stream, contentType, filename);
    }

    //[HttpPost]
    //public async Task<IActionResult> UpsertClient([FromBody]Client c)
    //{
    //  string clientId = User.GetClientId();
    //  await this._usersService.UpsertClient(clientId, c);
    //  return Ok();
    //}

  }
}
