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
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using AnnualReports.Model;

namespace AnnualReportsAPI.Controllers
{
  [Route("api/v.1/[controller]")]
  [CustomExceptionFilter]
  [Authorize]
  public class GfiController : Controller
  {
    IHostingEnvironment _env;
    private readonly GfiUploadOptions _gfiUploadOptions;
    private UsersService _usersService;
    private FileStorageService _fileStorageService;
    private ILogger _logger;

    public GfiController(IHostingEnvironment env,
                          IOptions<GfiUploadOptions> options,
                            UsersService usersService,
                              FileStorageService fileStorageService,
                              ILoggerFactory loggerFactory)
    {
      _env = env;
      _usersService = usersService;
      _gfiUploadOptions = options.Value;
      _fileStorageService = fileStorageService;
      _logger = loggerFactory.CreateLogger<GfiController>();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadGfiDocument(IFormFile gfiExcel)
    {
      string clientId = User.GetClientId();
      GodisnjiIzvjestaj gi = null;

      try
      {
        if (gfiExcel.Length <= 0)
        {
          throw new Exception("Empty gfi file.");
        }
        Stream s1 = new MemoryStream();
        Stream s2 = new MemoryStream();

        await gfiExcel.CopyToAsync(s1);
        await gfiExcel.CopyToAsync(s2);
        s1.Position = 0;
        s2.Position = 0;

        gi = GodisnjiIzvjestaj.LoadFromGFI(s1);

        Client c = new Client()
        {
          Oib = gi.Obveznik.OIB,
          Name = gi.Obveznik.Naziv,
          City = gi.Obveznik.NazivGradaIliOpcine,
          County = gi.Obveznik.NazivZupanije,
          Place = gi.Obveznik.NazivNaselja
        };

        GfiUpload gfi = new GfiUpload()
        {
          Filename = $"GFI-{gi?.Obveznik?.OIB}-{gi?.Godina.ToString()}.xls",
          Year = gi.Godina,
          ActivityCode = gi.Obveznik.NKDSifra,
          ActivityName = gi.Obveznik.NKDOpis,
          CompanyName = gi.Obveznik.Naziv,
          Oib = gi.Obveznik.OIB,
          Period = $"{gi.DatumOd.ToString("dd.MM.yyyy.")} - {gi.DatumDo.ToString("dd.MM.yyyy.")}",
          SubjectTypeCode = gi.Obveznik.SifraVrstePoslovnogSubjekta,
          SubjectTypeName = gi.Obveznik.NazivVrstePoslovnogSubjekta
        };

        await this._usersService.AddGfiToClient(clientId, c, gfi);

        s2.Position = 0;
        await this._fileStorageService.UploadFile(s2, clientId, gfi.Filename);
      }
      catch (Exception ex)
      {
        return BadRequest(new { Message = $"Došlo je do pogreške. [Output]: {ex.Message}" });
      }

      return Ok(new { Oib = gi.Obveznik.OIB, Year = gi.Godina });
    }
  }
}
