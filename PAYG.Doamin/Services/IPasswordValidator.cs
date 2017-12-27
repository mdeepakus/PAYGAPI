using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Services
{
    public interface IPasswordValidator
    {
        bool Validate(string password);
    }
}
