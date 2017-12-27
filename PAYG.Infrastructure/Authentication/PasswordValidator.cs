using PAYG.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Infrastructure.Authentication
{
    public class PasswordValidator : IPasswordValidator
    {
        private int requiredLength = 8;
        private bool requireNonAlphanumeric = true;
        private bool requireDigit = true;
        private bool requireLowercase = true;
        private bool requireUppercase = true;

        private List<string> errors = new List<string>();

        public bool Validate(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length < requiredLength)
            {
                errors.Add("Password Too Short");
            }

            if (requireNonAlphanumeric && password.All(IsLetterOrDigit))
            {
                errors.Add("Password Requires Non Alphanumeric");
            }

            if (requireDigit && !password.Any(IsDigit))
            {
                errors.Add("Password Requires Digit");
            }

            if (requireLowercase && !password.Any(IsLower))
            {
                errors.Add("Password Requires Lowercase");
            }

            if (requireUppercase && !password.Any(IsUpper))
            {
                errors.Add("Password Requires Uppercase");
            }

            return
                errors.Count == 0;
        }

        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        private bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        private bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}
