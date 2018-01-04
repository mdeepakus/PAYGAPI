using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PAYG.Domain.Common;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace PAYG.Server.Infrastructure.Filters
{
    /// <summary>
    /// 
    /// </summary>
    public class HandleErrorFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            ApiError apiError = null;

            if (context.Exception is ApiException)
            {
                var ex = context.Exception as ApiException;
                context.Exception = null;
                apiError = new ApiError(ex.Message);
                apiError.Errors = ex.Errors;

                context.HttpContext.Response.StatusCode = ex.StatusCode;
            }
            else if (context.Exception is ApiValidationException)
            {
                var ex = context.Exception as ApiValidationException;
                context.Exception = null;
                apiError = new ApiError("Validation Error");
                apiError.Errors = ex.Errors;

                context.HttpContext.Response.StatusCode = 400;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                var ex = context.Exception as UnauthorizedAccessException;
                apiError = new ApiError(ex.Message);
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                //TODO: Log unhandled errors in event log
#if !DEBUG
                var msg = "An unhandled error occurred.";
                string stack = null;
#else
                var msg = context.Exception.GetBaseException().Message;
                string stack = context.Exception.StackTrace;
#endif

                apiError = new ApiError("An unhandled error occurred.");
                apiError.Detail = stack;

                context.HttpContext.Response.StatusCode = 500;
            }

            // return JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
