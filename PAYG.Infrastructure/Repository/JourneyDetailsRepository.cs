using PAYG.Domain.Entities;
using PAYG.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Repository
{
    public class JourneyDetailsRepository : IJourneyDetailsRepository
    {
        private readonly IDataRepository _dataRepository;
        public JourneyDetailsRepository(IDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        public async Task Add(JourneyDetails journeyDetails, int journeyId)
        {
            var sql = @"INSERT INTO JourneyDetails
                (
                    journey_id,
                    longitude,
					latitude
                )
                VALUES
                (
                    @journey_id,
                    @longitude,
                    @latitude
                )";
            var parameters = new
            {
                journey_id = journeyId,
                longitude = journeyDetails.Longitude,
                latitude = journeyDetails.Latitude
            };

            await _dataRepository.ExecuteAsync(sql, parameters);

        }

        public Task<List<JourneyDetails>> GetList(int journeyId)
        {
            throw new NotImplementedException();
        }
    }
}
