using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using PAYG.Domain.Common;
using PAYG.Domain.Entities;
using PAYG.Domain.RepositoryInterfaces;
using PAYG.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PureBroker.Domain.Services
{
    public class ContextAccessor : IContextAccessor
    {
        private readonly ClaimsPrincipal _currentUser;
        private readonly IUserRepository _userRepository;
        private ApplicationUser _currentUserField = null;

        public ContextAccessor(IHttpContextAccessor contextAccessor, IUserRepository userRepository)
        {
            _currentUser = contextAccessor?.HttpContext?.User;
            _userRepository = userRepository;
            
            int userId = GetUserId();
            string role = GetClaimValue(ClaimTypes.Role);

            CurrentUser = _userRepository.GetApplicationUser(userId).Result;
        }

        public ContextAccessor(ApplicationUser currentUser)
        {
            CurrentUser = currentUser;
        }

        public ApplicationUser CurrentUser
        {
            get
            {
                if (_currentUserField == null)
                {
                    throw new UnauthorizedAccessException("Invalid login credentials");
                }

                return _currentUserField;
            }

            private set
            {
                _currentUserField = value;
            }
        }

        private int GetUserId()
        {
            var identityName = _currentUser?.Identity?.Name;
            int userId = 0;

            if (!string.IsNullOrWhiteSpace(identityName))
            {
                userId = int.Parse(identityName);
            }

            return userId;
        }

        private string GetClaimValue(string claimType)
        {
            string claimValue = string.Empty;
            claimValue = _currentUser?.Claims?.SingleOrDefault(c => c.Type == claimType).Value;
            return claimValue;
        }
    }
}
