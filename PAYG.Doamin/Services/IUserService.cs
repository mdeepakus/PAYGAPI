using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace PAYG.Domain.Services
{
    public interface IUserService
    {
        Task<ActionResult<ApplicationUser>> UserLogon(string username, string password);

        Task<int> RegisterConsumerUser(string userName, string password, string confirmPassword, RegisterNewUser userDetails);
    }
}
