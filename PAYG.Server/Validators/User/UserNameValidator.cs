using PAYG.Domain.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Validators.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserNameValidator : IUserValidator
    {
        private readonly IUserRepository _userRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        public UserNameValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<bool> ValidateUserExists(string userName)
        {
            bool result = false;

            if (userName.Length > 0 && !await _userRepository.UserExists(userName))
            {
                result = true;
            }

            return await Task.FromResult(result);

        }
    }
}
