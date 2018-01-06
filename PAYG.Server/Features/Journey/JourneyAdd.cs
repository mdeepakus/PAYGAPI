using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PAYG.Domain.Entities;
using PAYG.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PAYG.Server.Features.Journey
{
    /// <summary>
    /// 
    /// </summary>
    public class JourneyAdd
    {
        /// <summary>
        /// 
        /// </summary>
        public class Command : IRequest<Result>
        {
            public Command()
            {
                VehicleCoordinates = new List<JourneyDetail>();
            }

            public int VehicleId { get; set; }
            public int UserId { get; set; }
            public DateTime? StarteDate { get; set; }
            public DateTime? EndDate { get; set; }
            /// <summary>
            /// List of corodinates
            /// </summary>
            public List<JourneyDetail> VehicleCoordinates { get; set; }
        }
        /// <summary>
        /// 
        /// </summary>
        public class Result
        {
            /// <summary>
            /// 
            /// </summary>
            public int Id { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class JourneyDetail
        {
            public string Longitude { get; set; }
            public string Latitude { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.UserId).GreaterThan(0).WithMessage("User id is mandatory");
                RuleFor(c => c.VehicleId).GreaterThan(0).WithMessage("Vehicle id is mandatory");
                RuleFor(c => c.StarteDate).NotEmpty().WithMessage("Start date is mandatory");
                RuleFor(c => c.EndDate).NotEmpty().WithMessage("End date is mandatory");
                RuleFor(c => c.VehicleCoordinates).NotEmpty();
                //yet to implement
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class Handler : IAsyncRequestHandler<Command, Result>
        {
            private IJourneyService _journeyService;
            public Handler(IJourneyService journeyService)
            {
                _journeyService = journeyService;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="message"></param>
            /// <returns></returns>
            public async Task<Result> Handle(Command message)
            {
                PAYG.Domain.Entities.Journey journey;

                journey = new Domain.Entities.Journey
                {
                    UserId = message.UserId,
                    VehicleId = message.VehicleId,
                    StarteDate = message.StarteDate,
                    EndDate = message.EndDate
                };

                var coordinates = new List<PAYG.Domain.Entities.JourneyDetails>();
                foreach (var listdata in message.VehicleCoordinates)
                {
                    var data = new PAYG.Domain.Entities.JourneyDetails();
                    data.Latitude = listdata.Latitude;
                    data.Longitude = listdata.Longitude;
                    coordinates.Add(data);
                }


                int journeyId = await _journeyService.AddJourney(coordinates, journey);
                return new Result { Id = journeyId };
            }
        }
    }
}
