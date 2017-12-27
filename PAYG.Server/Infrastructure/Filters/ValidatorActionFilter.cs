using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PAYG.Domain.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidatorActionFilter : IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        [DebuggerStepThrough]
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.EnableRewind();

            if (!context.ModelState.IsValid)
            {
                throw new ApiException("Validation Error", 400, GetValidationErrors(context.ModelState));
            }
        }
        private List<ValidationError> GetValidationErrors(ModelStateDictionary modelState)
        {
            List<ValidationError> errors = new List<ValidationError>();

            if (modelState != null && modelState.Any(m => m.Value.Errors.Count > 0))
            {
                errors = modelState.SelectMany(m => m.Value.Errors.Select(ms => new ValidationError(ms.ErrorMessage, m.Key))).ToList();
            }

            return errors;
        }
    }
}
