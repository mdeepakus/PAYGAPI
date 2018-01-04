using MediatR;
using Microsoft.AspNetCore.Mvc;
using PAYG.Domain.Common;
using PAYG.Server.Infrastructure.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Features
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [HandleErrorFilter]
    [SwaggerResponse(400, typeof(ApiError), "Validation Error")]
    public abstract class BaseController : Controller
    {
        public BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }

        protected IMediator Mediator { get; }
    }
}
