using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace AnnualReportsAPI.Models
{
  public class ApplicationUser
  {
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
  }

  public class PasswordRecover
  {
    [Required]
    public string password { get; set; }
    [Required]
    public string recoverToken { get; set; }
  }

  public class RecoverTokenPost
  {
    [Required]
    public string Email { get; set; }
  }
}
