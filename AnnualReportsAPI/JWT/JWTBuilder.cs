using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace AnnualReportsAPI.JWT
{
  public class JWTBuilder
  {
    private string _user;
    private string _clientId;
    private JwtIssuerOptions _jwtOptions;
    private IEnumerable<string> _roles;

    public JWTBuilder(string user, string clientIdGuid, JwtIssuerOptions options, IEnumerable<string> roles)
    {
      _user = user;
      _jwtOptions = options;
      _roles = roles;
      _clientId = clientIdGuid;
    }

    public string GetJWT()
    {
      var identity = new ClaimsIdentity();
            Console.Write(_jwtOptions.Issuer);
      List<Claim> claims = new List<Claim>()
      {
        new Claim(JwtRegisteredClaimNames.Sub, _user),
        new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator().Result),
        new Claim(JwtRegisteredClaimNames.Iat,
                  ToUnixEpochDate(_jwtOptions.IssuedAt).ToString(),
                  ClaimValueTypes.Integer64),
        identity.FindFirst(_user)
      };


      Claim usernameClaim = new Claim("email", _user);
      claims.Add(usernameClaim);

      Claim clientIdClaim = new Claim("clientId", _clientId);
      claims.Add(clientIdClaim);

      foreach (string role in _roles)
      {
        Claim r = new Claim("roles", role);
        claims.Add(r);
      }

      // Create the JWT security token and encode it.
      var jwt = new JwtSecurityToken(
          issuer: _jwtOptions.Issuer,
          audience: _jwtOptions.Audience,
          claims: claims,
          notBefore: _jwtOptions.NotBefore,
          expires: _jwtOptions.Expiration,
          signingCredentials: _jwtOptions.SigningCredentials);

      return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public static void ThrowIfInvalidOptions(JwtIssuerOptions options)
    {
      if (options == null) throw new ArgumentNullException(nameof(options));

      if (options.ValidFor <= TimeSpan.Zero)
      {
        throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
      }

      if (options.SigningCredentials == null)
      {
        throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
      }

      if (options.JtiGenerator == null)
      {
        throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
      }
    }

    private static long ToUnixEpochDate(DateTime date)
      => (long)Math.Round((date.ToUniversalTime() -
                           new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                          .TotalSeconds);
  }
}
