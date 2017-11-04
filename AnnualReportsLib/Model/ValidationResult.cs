using System;
using System.Collections.Generic;

namespace AnnualReports.Model
{
    public class ValidationResult
    {

        public Result Result { get; set; }

        public List<string> Errors { get; set; }

        public ValidationResult()
        {
        }
    }

    public enum Result { OK, INVALID }
}
