using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AnnualReportsAPI.ErrorResponses
{
  public class ValidationError
  {
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Field { get; }

    public string Message { get; }

    public ValidationError(string field, string message)
    {
      Field = field != string.Empty ? ValidationResultModel.ToFirstLetterLower(field) : null;
      Message = message;
    }
  }

  public class ValidationResultModel
  {
    public string Message { get; }

    public List<ValidationError> Errors { get; }

    public ValidationResultModel(ModelStateDictionary modelState)
    {
      Message = "Validation Failed";
      Errors = modelState.Keys
              .SelectMany(key => modelState[key].Errors.Select(x =>
              {
                string errorMessage = "Error occured.";
                if (x.ErrorMessage != null && x.ErrorMessage != String.Empty)
                  errorMessage = x.ErrorMessage;
                else if (x.Exception?.Message != null)
                  errorMessage = x.Exception.Message;

                return new ValidationError(key, errorMessage);
              }))
              .ToList();
    }

    public static string ToFirstLetterLower(string text)
    {
      var charArray = text.ToCharArray();
      charArray[0] = char.ToLower(charArray[0]);

      return new string(charArray); }
  }
}
