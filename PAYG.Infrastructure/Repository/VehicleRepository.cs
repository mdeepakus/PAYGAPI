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
        public async Task<Vehicle> Add(Vehicle vehicle, int userId)
        {
            Ensure.ArgumentNotNull(vehicle, nameof(vehicle));
            Ensure.ArgumentNotNull(userId, nameof(userId));

            var sql =
                @"DECLARE @Vehicle_id INT
                INSERT INTO Vehicle
                    ([registrationnumber]
                    ,[make]
                    ,[model]
                    ,[type]
                    ,[user_id])
                 VALUES
                    (
                    @registrationnumber,
                    @make,
                    @model,
                    @type,
                    @user_id
                    );
                    
                    SELECT @Vehicle_id = SCOPE_IDENTITY();";

            var parameters = new
            {
                registrationnumber = vehicle.RegistrationNumber,
                make = vehicle.Make,
                model = vehicle.Model,
                type = vehicle.Type,
                user_id = userId
            };

            var vehicleId = await _dataRepository.ExecuteScalar<int>(sql, parameters);
            var vehicledetails = await Get(vehicleId);


            return vehicledetails;

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
                "select vehicleId, registrationnumber, make, model, type " +
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
                Model = vehicle.model,
                Type = vehicle.type
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
