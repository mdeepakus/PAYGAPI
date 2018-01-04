using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Domain.Services
{
    public interface IJourneyService
    {
        Task<int> AddJourney(List<JourneyDetails> journeyDetails, Journey journey);
    }
}
