using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using PAYG.Domain.RepositoryInterfaces;
using PAYG.Domain.Extensions;

namespace PAYG.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IPasswordValidator _passwordValidator;

        public UserService(IUserRepository repository, IPasswordHasher passwordHasher, IPasswordValidator passwordValidator)
        {
            _repository = repository;
            _passwordHasher = passwordHasher;
            _passwordValidator = passwordValidator;
        }

        public async Task<ActionResult<ApplicationUser>> UserLogon(string username, string password)
        {
            Ensure.ArgumentNotNull(username, nameof(username));
            Ensure.ArgumentNotNull(password, nameof(password));

            var user = await _repository.GetApplicationUser(username);

            if (user == null)
            {
                return FailActionResult("Username / Password Incorrect.", null);
            }

            if (!IsPasswordCorrect(password, user.Password))
            {
                return FailActionResult("Username / Password Incorrect.", user);
            }

            return new ActionResult<ApplicationUser>() { Object = user, Result = Result.Success };
        }

        private static ActionResult<ApplicationUser> FailActionResult(string message, ApplicationUser user)
        {
            return new ActionResult<ApplicationUser>()
            {
                Object = user,
                Result = Result.Fail,
                ErrorMessage = message
            };
        }

        private bool IsPasswordCorrect(string testPassword, string accountPassword)
        {     
            return _passwordHasher.IsPasswordValid(accountPassword, testPassword);
        }

        public async Task<int> RegisterConsumerUser(string userName, string password, string confirmPassword, RegisterNewUser userDetails)
        {
            Ensure.ArgumentNotNull(userName, nameof(userName));
            Ensure.ArgumentNotNull(password, nameof(password));
            Ensure.ArgumentNotNull(confirmPassword, nameof(confirmPassword));

            if (!_passwordValidator.Validate(password))
            {
                throw new ApiException("Password does not meet complexity requirements.");
            }

            int userId = await _repository.CreateConsumerUser(userName, _passwordHasher.Hash(password), userDetails);

            return userId;
        }
    }
}
