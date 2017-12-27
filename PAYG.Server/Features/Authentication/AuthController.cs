using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PAYG.Domain.Common;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Features.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/auth")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, description: "Authentication error", Type = typeof(ApiError))]
    public class AuthController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public AuthController(IMediator mediator)
            : base(mediator)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, typeof(Login.Result), "Successful Login")]
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody]Login.Command command)
        {
            var response = await Mediator.Send(command);

            if (string.IsNullOrEmpty(response.Token))
            {
                ApiError apiError = new ApiError(response.ActionMessage);
                apiError.Errors = new List<ValidationError>();

                return StatusCode(StatusCodes.Status401Unauthorized, apiError);
            }

            return Ok(response);
        }

        /// <summary>
        /// Logs out of the application
        /// </summary>
        /// <returns>Ok</returns>
        [SwaggerResponse(StatusCodes.Status200OK)]
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // do nothing this is currently a dummy routine (will be added later)

            return Ok();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(Login.Result))]
        [HttpPost("regiterNewUser")]
        [AllowAnonymous]
        public async Task<IActionResult>RegisterNewUser([FromBody]RegisterUser.Command command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }
    }
}
