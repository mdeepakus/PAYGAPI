using Microsoft.IdentityModel.Tokens;
using PAYG.Domain.Common;
using System;

namespace PAYG.Infrastructure.Authentication
{
    public class TokenProviderOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        // ** if (!Environment.MachineName.Equals("IN00268")) to be deleted before publishing a Release version **
        public TimeSpan Expiration
        {
            get
            {
                var timeSpan = Environment.MachineName.Equals("IN00268")
                    ? TimeSpan.FromDays(31)
                    : TimeSpan.FromMinutes(60);

                return timeSpan;
            }
        }

        public TimeSpan ClockSkew { get; set; } = TimeSpan.FromMinutes(0);

        public SigningCredentials SigningCredentials { get; set; }
    }
}
