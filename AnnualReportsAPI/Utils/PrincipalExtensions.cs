using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;

namespace AnnualReportsAPI.Utils
{
  public static class PrincipalExtensions
  {
    public static string GetUsername(this IPrincipal principal)
    {
      Claim usernameClaim = ((ClaimsIdentity)principal.Identity).Claims.FirstOrDefault(x => x.Type == "email");
      if (usernameClaim == null)
        return null;

      return usernameClaim.Value;
    }
  }
}
