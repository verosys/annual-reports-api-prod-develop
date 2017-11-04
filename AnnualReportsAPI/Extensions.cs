using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Security.Principal;

namespace AnnualReportsAPI.Extensions
{
  public static class PrincipalExtensions
  {
    public static string GetEmail(this IPrincipal principal)
    {
      Claim usernameClaim = ((ClaimsIdentity)principal.Identity).Claims.FirstOrDefault(x => x.Type == ClaimValueTypes.Email);
      if (usernameClaim == null)
        return null;

      return usernameClaim.Value;
    }
    public static string GetClientId(this IPrincipal principal)
    {
      Claim usernameClaim = ((ClaimsIdentity)principal.Identity).Claims.FirstOrDefault(x => x.Type == "clientId");
      if (usernameClaim == null)
        return null;

      return usernameClaim.Value;
    }
  }
}
