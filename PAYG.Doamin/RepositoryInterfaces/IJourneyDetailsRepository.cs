using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Domain.RepositoryInterfaces
{
    public interface IJourneyDetailsRepository
    {
        Task<List<JourneyDetails>> GetList(int journeyId);

        Task Add(JourneyDetails journeyDetails, int journeyId);
    }
}
