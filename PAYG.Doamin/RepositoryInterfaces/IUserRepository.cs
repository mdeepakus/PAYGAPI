using System;
using PAYG.Domain.Entities;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Domain.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<ApplicationUser> GetApplicationUser(int userId);

        Task<ApplicationUser> GetApplicationUser(string userName);

        Task<int> CreateConsumerUser(string userName, string hashedPassword, RegisterNewUser userDetails);

        Task<bool> UserExists(string userName);
    }
}
