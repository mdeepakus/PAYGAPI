using PAYG.Domain.Entities;
using PAYG.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Dapper;
using PAYG.Domain.Extensions;

namespace PAYG.Infrastructure.Repository
{
    public class VehicleRepository : IVehiclesRepository
    {
        private readonly IDataRepository _dataRepository;

        public VehicleRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        public Task<Vehicle> Add(Vehicle vehicle, int userId)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int vehicleId, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Vehicle> Get(int vehicleId)
        {
            //throw new NotImplementedException();
            Ensure.ArgumentNotNull(vehicleId, nameof(vehicleId));

            var sql =
                "select vehicleId, registrationnumber, make, model " +
                "from Vehicle where vehicleid = @vehicleId";

            var data = await  _dataRepository.QueryAsync(sql, new { vehicleId });

            var vehicle = data.SingleOrDefault();

            if (vehicle == null)
                return null; 
            
            return new Vehicle
            {
                Id = vehicle.vehicleId,
                RegistrationNumber = vehicle.registrationnumber,
                Make = vehicle.make,
                Model = vehicle.model
            };
        }

        public Task<List<Vehicle>> GetList(int userId)
        {
            throw new NotImplementedException();
        }

        public Task Update(Vehicle vehicle, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
