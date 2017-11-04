using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnnualReportsAPI.Utils
{
  public static class SecurityUtils
  {
    public static string EncryptPassword(string input)
    {
      var salt = GenerateSalt(17);
      var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 256/8);

      return $"{ Convert.ToBase64String(salt) }:{ Convert.ToBase64String(bytes) }";
    }

    private static byte[] GenerateSalt(int length)
    {
      var salt = new byte[length];
      using (var random = RandomNumberGenerator.Create())
      {
        random.GetBytes(salt);
      }

      return salt;
    }

    public static bool CheckMatch(string hash, string input)
    {
      try
      {
        var parts = hash.Split(':');
        var salt = Convert.FromBase64String(parts[0]);
        var bytes = KeyDerivation.Pbkdf2(input, salt, KeyDerivationPrf.HMACSHA512, 10000, 256 / 8);

        return parts[1].Equals(Convert.ToBase64String(bytes));
      }
      catch
      {
        return false;
      }
    }

    public static string GeneratePasswordRecoveryTokenString(int length, bool urlEncoded = false)
    {
      using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
      {
        byte[] tokenData = new byte[length];
        rng.GetBytes(tokenData);

        string token = Convert.ToBase64String(tokenData)
        .Replace("+", "")
        .Replace("/", "");

        if (urlEncoded == true)
          token = System.Net.WebUtility.UrlEncode(token);

        return token;
      }
    }
  }
}
