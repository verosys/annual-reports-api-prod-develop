using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using AnnualReportsAPI.JWT;
using System.Collections.Generic;
using System.Linq;
using AnnualReportsAPI.ErrorResponses;
using Microsoft.AspNetCore.Http;
using AnnualReportsAPI.Utils;
using AnnualReportsAPI.Services;
using System.ComponentModel.DataAnnotations;
using AnnualReportsAPI.Models;

namespace AnnualReportsAPI.Controllers
{
  [Route("api/v.1/[controller]")]
  [CustomExceptionFilter]
  public class JwtController : Controller
  {
    private readonly JwtIssuerOptions _jwtOptions;
    private readonly ILogger _logger;
    private readonly JsonSerializerSettings _serializerSettings;
    private UsersService _usersService;

    public JwtController(IOptions<JwtIssuerOptions> jwtOptions,
                          ILoggerFactory loggerFactory,
                            UsersService usersService)
    {
      _usersService = usersService;
      _jwtOptions = jwtOptions.Value;
      JWTBuilder.ThrowIfInvalidOptions(_jwtOptions);

      _logger = loggerFactory.CreateLogger<JwtController>();

      _serializerSettings = new JsonSerializerSettings
      {
        Formatting = Formatting.Indented
      };
    }

    [HttpPost("register")]
    [ValidateModel]
    public async Task<IActionResult> Register([FromBody] RegisterDetails details)
    {
      var u = await this._usersService.FindByEmail(details.Email);

      if (u != null)
      {
        return BadRequest(new { Message = "Korisnik već postoji." });
      }

      Models.User user = new Models.User()
      {
        CompanyName = details.CompanyName,
        Email = details.Email,
        ClientId = Guid.NewGuid().ToString("N").ToLower(),
        Contact = details.Contact,
        IsEmailConfirmed = false,
        Password = SecurityUtils.EncryptPassword(details.Password),
        Phone = details.Phone,
        Roles = new string[] { "User" }
      };

      Models.User createdUser = await _usersService.CreateNew(user);

      if (createdUser == null)
      {
        _logger.LogInformation($"Could not register ({details.Email})");
        return StatusCode(400, new { Message = "Došlo je do pogreške" });
      }
      JWTBuilder jwtBuild = new JWTBuilder(createdUser.Email, createdUser.ClientId, _jwtOptions, createdUser.Roles);
      string encodedJwt = jwtBuild.GetJWT();

      var userProfile = new
      {
        ClientId = createdUser.ClientId,
        Email = createdUser.Email
      };

      // Serialize and return the response
      var tokenWrapper = new
      {
        token = encodedJwt,
        user = userProfile
      };

      //Response.Cookies.Append(
      //  "refreshToken",
      //  "testRefreshTokenContent",
      //  new CookieOptions()
      //  {
      //    HttpOnly = true
      //  });

      var json = JsonConvert.SerializeObject(tokenWrapper, _serializerSettings);
      return Ok(tokenWrapper);
    }

    [HttpPost]
    [ValidateModel]
    public async Task<IActionResult> Get([FromBody] ApplicationUser applicationUser)
    {
      var userFound = await this._usersService.FindByEmailPassword(applicationUser.Email, applicationUser.Password);

      if (userFound == null)
      {
        _logger.LogInformation($"Invalid username ({applicationUser.Email}) or password ({applicationUser.Password})");
        return StatusCode(422, new { Message = "Neispravna email adresa ili lozinka" });
      }

      JWTBuilder jwtBuild = new JWTBuilder(userFound.Email, userFound.ClientId, _jwtOptions, userFound.Roles);
      string encodedJwt = jwtBuild.GetJWT();

      var userProfile = new
      {
        ClientId = userFound.ClientId,
        Email = userFound.Email
      };

      // Serialize and return the response
      var tokenWrapper = new
      {
        token = encodedJwt,
        user = userProfile
      };

      //Response.Cookies.Append(
      //  "refreshToken",
      //  "testRefreshTokenContent",
      //  new CookieOptions()
      //  {
      //    HttpOnly = true
      //  });

      var json = JsonConvert.SerializeObject(tokenWrapper, _serializerSettings);
      return Ok(tokenWrapper);
    }

    [HttpPost("createrecovertoken")]
    [ValidateModel]
    public async Task<IActionResult> CreateRecoverToken([FromBody] RecoverTokenPost recover)
    {
      string token = await this._usersService.CreateRecoverToken(recover.Email);
      if (!String.IsNullOrEmpty(token))
      {
        //TODO: Send email with token
      }
      return NoContent();
    }

    [HttpPost("resetpassword")]
    [ValidateModel]
    public async Task<IActionResult> ResetPassword([FromBody] PasswordRecover recover)
    {
      await this._usersService.ResetPassword(recover.recoverToken, recover.password);
      return NoContent();
    }

    private static long ToUnixEpochDate(DateTime date)
      => (long)Math.Round((date.ToUniversalTime() -
                           new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                          .TotalSeconds);
  }
}