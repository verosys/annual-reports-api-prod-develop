using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AnnualReportsAPI.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AnnualReportsAPI.ErrorResponses
{
  public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
  {
    private ILogger _logger;

    public CustomExceptionFilterAttribute()
    {
      
    }
    public override void OnException(ExceptionContext context)
    {
      ILoggerFactory loggerFactory = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>();
      _logger = loggerFactory.CreateLogger<CustomExceptionFilterAttribute>();

      if (context.Exception is ArgumentNullException)
      {
        context.Result = new BadRequestObjectResult(new { Message = context.Exception.Message });
      }

      //if(context.Exception is ServiceValidationException)
      //{
      //  context.Result = new BadRequestObjectResult(new { Message = context.Exception.Message });
      //}

      if (context.Exception is ServiceOperationException)
      {
        context.Result = new BadRequestObjectResult(new { Message = context.Exception.Message });
      }

      else
      {
        context.Result = new BadRequestObjectResult(new { Message = "Došlo je do pogreške." });
      }

      this._logger.LogError($"Exception occured: {context.Exception.Message}");

      base.OnException(context);
    }
  }
}
