using PAYG.Domain.Entities;
using PAYG.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Repository
{
    public class JourneyRepository : IJourneyRepository
    {
        private readonly IDataRepository _dataRepository;

        public JourneyRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }
        public async Task<int> Add(Journey journey)
        {
            string sql = @"DECLARE @journey_id INT

                INSERT INTO Journey
                (
                    Vehicle_id,
					user_id,
					Startdate,
					Enddate
                )
                VALUES
                (
                    @vehicle_id,
                    @user_id,
                    @start_date,
                    @end_date
                )
                SELECT @journey_id = @@IDENTITY
                
                SELECT @journey_id";

            var parameters = new
            {
                vehicle_id = journey.VehicleId,
                user_id = journey.UserId,
                start_date = journey.StarteDate,
                end_date = journey.EndDate

            };

            var data = await _dataRepository.QueryAsync<int>(sql, parameters);
            return data.FirstOrDefault();
        }

        public Task<Journey> Get(int vehicleId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Journey>> GetList(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
