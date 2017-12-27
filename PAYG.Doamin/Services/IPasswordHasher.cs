using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Services
{
    public interface IPasswordHasher
    {
        string Hash(string password);

        bool IsPasswordValid(string hashedPassword, string password);
    }
}
