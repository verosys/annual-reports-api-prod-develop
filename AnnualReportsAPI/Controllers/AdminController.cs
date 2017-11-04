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

namespace AnnualReportsAPI.Controllers
{
  [Route("api/v.1/[controller]")]
  [CustomExceptionFilter]
  [Authorize(Roles = "Admin")]
  public class AdminController : Controller
  {
    IHostingEnvironment _env;
    private UsersService _usersService;

    public AdminController(IHostingEnvironment env,
                              UsersService usersService)
    {
      _env = env;
      _usersService = usersService;
    }

    [HttpGet("customers")]
    public async Task<IActionResult> ListUsers(string q = null, int? page = 1, int? pageSize = 7)
    {
      string clientId = User.GetClientId();
      return Ok(await this._usersService.SearchUsers(q, page, pageSize));
    }

    [HttpGet("customers/{email}")]
    public async Task<IActionResult> FindUserByEmail(string email)
    {
      if (String.IsNullOrEmpty(email))
        return BadRequest(new { Message = "User unknown." });

      return Ok(await this._usersService.FindByEmail(email));
    }
  }
}
