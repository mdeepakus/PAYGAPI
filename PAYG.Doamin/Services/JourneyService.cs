using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using PAYG.Domain.Extensions;
using PAYG.Domain.RepositoryInterfaces;

namespace PAYG.Domain.Services
{
    public class JourneyService : IJourneyService
    {
        private readonly IJourneyRepository _journeyRepository;
      
        public JourneyService(IJourneyRepository journey)
        {
            _journeyRepository = journey;
        }
        public async Task<int> AddJourney(List<JourneyDetails> journeyDetails, Journey journey)
        {
            int journeyId;
            try
            {
                journeyId = await _journeyRepository.Add(journey);
                if (journeyId > 0)
                {
                    foreach (JourneyDetails details in journeyDetails)
                    {
                        await _journeyRepository.AddJourneyDetails(details, journeyId);
                    }
                }
                else
                {
                    throw new ApiException("Failed to save journey!!!");
                }
            }
            catch(Exception ex)
            {
                throw new ApiException(ex);
            }
            
            return journeyId;
        }
    }
}
