using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;


namespace PAYG.Server.Features.Journey
{
    /// <summary>
    /// 
    /// </summary>
    public class JourneyController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public JourneyController(IMediator mediator)
            :base(mediator)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(JourneyAdd.Result))]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserJourney([FromBody]JourneyAdd.Command command)
        {
            var response = await Mediator.Send(command);

            return Ok(response);
        }
    }
}
