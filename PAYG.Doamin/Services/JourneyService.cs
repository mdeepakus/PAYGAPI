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
        private readonly IJourneyDetailsRepository _journeyDetailsRepository;

        public JourneyService(IJourneyRepository journey, IJourneyDetailsRepository journeyDetailsRepository)
        {
            _journeyRepository = journey;
            _journeyDetailsRepository = journeyDetailsRepository;
        }
        public async Task<int> AddJourney(List<JourneyDetails> journeyDetails, Journey journey)
        {
            int journeyId = await _journeyRepository.Add(journey);
            if (journeyId > 0)
            {
                foreach (JourneyDetails details in journeyDetails)
                {
                    await _journeyDetailsRepository.Add(details, journeyId);
                }
            }
            else
            {
                throw new ApiException("Failed to save journey!!!"); 
            }

            return journeyId;
        }
    }
}
