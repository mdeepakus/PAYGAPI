using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Common
{
    public class ApiException : Exception
    {
        public ApiException(string message, int statusCode = 400, List<ValidationError> errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            Errors = errors ?? new List<ValidationError>();
        }

        public ApiException(Exception ex, int statusCode = 400)
            : base(ex.Message)
        {
            StatusCode = statusCode;
        }

        public int StatusCode { get; set; }

        public List<ValidationError> Errors { get; set; }
    }
}
