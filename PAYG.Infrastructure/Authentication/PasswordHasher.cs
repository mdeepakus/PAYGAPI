using PAYG.Domain.Entities;
using PAYG.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Authentication
{
    public class PasswordHasher : IPasswordHasher
    {
        private Microsoft.AspNetCore.Identity.IPasswordHasher<ApplicationUser> _passwordHasher;

        public PasswordHasher()
        {
            this._passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<ApplicationUser>();
        }

        public string Hash(string password)
        {
            return _passwordHasher.HashPassword(null, password);
        }

        public bool IsPasswordValid(string hashedPassword, string password)
        {
            try
            {
                var verified = _passwordHasher.VerifyHashedPassword(null, hashedPassword, password);
                return verified == Microsoft.AspNetCore.Identity.PasswordVerificationResult.Success;
            }

            // if hashed password is not a base64 encoded string
            catch (System.FormatException) 
            {
                return false;
            }
        }
    }
}
