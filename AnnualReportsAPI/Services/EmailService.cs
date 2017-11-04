using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using AnnualReportsAPI.Options;
using AnnualReportsAPI.Exceptions;

namespace AnnualReportsAPI.Services
{
  public class EmailService
  {
    private SendGridOptions _sendGridOptions;

    public EmailService(IOptions<SendGridOptions> options)
    {
      _sendGridOptions = options.Value;
    }

    public async Task Send(string recipientAddress, string subject, string plainTextContent, string htmlContent)
    {
      var client = new SendGridClient(this._sendGridOptions.ApiKey);
      var from = new EmailAddress(this._sendGridOptions.FromAddress, this._sendGridOptions.FromTitle);
      var to = new EmailAddress(recipientAddress);
      var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
      var response = await client.SendEmailAsync(msg);
    }
  }
}
