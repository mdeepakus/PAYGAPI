using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace PAYG.Server.Features.Vehicles
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class VehiclesController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public VehiclesController(IMediator mediator)
            :base(mediator)
        {

        }

        /// <summary>
        /// Retrieve a single vehicle
        /// </summary>
        /// <param name="query">query parameter</param>
        /// <returns>Client details</returns>
        [SwaggerResponse(StatusCodes.Status200OK, typeof(GetVehicle.Result))]
        [HttpGet("{vehicleId:int}", Name = "GetVehicle")]
        public async Task<IActionResult> GetVehicle(GetVehicle.Query query)
        {
            var result = await Mediator.Send(query);

            return Ok(result);
        }
    }
}
