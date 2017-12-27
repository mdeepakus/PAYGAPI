using MediatR;
using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using PAYG.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Features.Vehicles
{
    public class GetVehicle
    {
        public class Query : VehicleQuery, IRequest<Result>
        {
            
        }

        public class Result
        {
            public Result()
            {

            }

            public int Id { get; set; }
            public string RegistrationNumber { get; set; }
            public string Make { get; set; }
            public string Model { get; set; }
        }

        public class Handler : IAsyncRequestHandler<Query, Result>
        {
            private readonly IVehiclesRepository _repository;

            public Handler(IVehiclesRepository repository)
            {
                _repository = repository;
            }
            public async Task<Result> Handle(Query query)
            {
                var data = await _repository.Get(query.VehicleID);

                if (data == null)
                {

                    return new Result();
                }

                var result = new Result
                {
                    Id = data.Id,
                    RegistrationNumber = data.RegistrationNumber,
                    Make = data.Make,
                    Model = data.Model
                };

                return await Task.FromResult(result);
            }
        }
    }
}
