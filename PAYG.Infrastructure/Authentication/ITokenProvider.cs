using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Authentication
{
    public interface ITokenProvider
    {
        string GenerateToken(ApplicationUser user);
    }
}
