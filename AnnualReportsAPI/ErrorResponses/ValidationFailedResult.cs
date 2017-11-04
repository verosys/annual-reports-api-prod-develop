using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;

namespace AnnualReportsAPI.ErrorResponses
{
  public class ValidationFailedResult : ObjectResult
  {
    public ValidationFailedResult(ModelStateDictionary modelState)
        : base(new ValidationResultModel(modelState))
    {
      StatusCode = StatusCodes.Status422UnprocessableEntity;
    }
  }
}
