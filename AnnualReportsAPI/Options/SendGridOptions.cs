using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AnnualReportsAPI.Options
{
  public class SendGridOptions
  {
    public string ApiKey { get; set; }
    public string FromAddress { get; set; }
    public string FromTitle { get; set; }
  }
}
