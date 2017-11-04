using System;
using System.Collections.Generic;
using System.Text;

namespace AnnualReportsAPI.Exceptions
{
  public class ServiceOperationException : Exception
  {
    public ServiceOperationException() { }
    public ServiceOperationException(string message) : base(message) { }
    public ServiceOperationException(string message, Exception innerException) : base(message, innerException) { }
  }
}
