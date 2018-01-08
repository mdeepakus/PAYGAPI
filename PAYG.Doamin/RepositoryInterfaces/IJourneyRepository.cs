using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Domain.RepositoryInterfaces
{
    public interface IJourneyRepository
    {
        Task<List<Journey>> GetList(int userId);

        Task<Journey> Get(int vehicleId, int userId);

        Task<int> Add(Journey journey);

        Task AddJourneyDetails(JourneyDetails journeyDetails, int journeyId);
        
    }
}
