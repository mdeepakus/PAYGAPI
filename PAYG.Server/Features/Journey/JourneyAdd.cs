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
                journeyDetails = new List<JourneyDetails>();
            }

            public int VehicleId { get; set; }
            public int UserId { get; set; }
            public DateTime? StarteDate { get; set; }
            public DateTime? EndDate { get; set; }
            public List<JourneyDetails> journeyDetails { get; set; }
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

        ///// <summary>
        ///// 
        ///// </summary>
        //public class JourneyDetail
        //{
        //    public int Id { get; set; }
        //    public string Logitude { get; set; }
        //    public string Latitude { get; set; }
        //}

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
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
                int journeyId = await _journeyService.AddJourney(message.journeyDetails, journey);
                return new Result { Id = journeyId };
            }
        }
    }
}
