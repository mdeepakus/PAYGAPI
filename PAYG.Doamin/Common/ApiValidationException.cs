using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Common
{
    public class ApiValidationException : Exception
    {
        public ApiValidationException(List<ValidationError> errors)
            : base()
        {
            Errors = errors;
        }

        public ApiValidationException(string validationMessage, string validationTarget)
            : base()
        {
            Errors = new List<ValidationError>
            {
                new ValidationError
                {
                    Message = validationMessage,
                    Target = validationTarget
                }
            };
        }

        public ApiValidationException(string validationMessage)
            : base()
        {
            Errors = new List<ValidationError>
            {
                new ValidationError
                {
                    Message = validationMessage
                }
            };
        }

        public List<ValidationError> Errors { get; set; }
    }
}
