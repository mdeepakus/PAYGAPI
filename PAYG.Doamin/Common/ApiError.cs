using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Common
{
    public class ApiError
    {
        public ApiError(string message)
        {
            Message = message;
            IsError = true;
        }

        public string Message { get; set; }

        public bool IsError { get; set; }

        public string Detail { get; set; }

        public List<ValidationError> Errors { get; set; }
    }
}
