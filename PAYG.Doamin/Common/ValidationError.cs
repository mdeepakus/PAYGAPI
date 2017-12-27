using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Common
{
    public class ValidationError
    {
        public ValidationError()
        {
        }

        public ValidationError(string message)
        {
            Message = message;
        }

        public ValidationError(string message, string target)
        {
            Message = message;
            Target = target;
        }

        public ValidationError(string message, string target, string id)
        {
            Message = message;
            Target = target;
            Id = id;
        }

        /// <summary>
        /// The error message for this validation error.
        /// </summary>
        /// <value>The error message</value>
        public string Message { get; set; }

        /// <summary>
        /// The name of the field that this error relates to.
        /// </summary>
        /// <value>Targe of the error</value>
        public string Target { get; set; }

        /// <summary>
        /// An ID set for the Error. This ID can be used as a correlation between bus object and UI code.
        /// </summary>
        /// <value>Id of the item</value>
        public string Id { get; set; }
    }
}
