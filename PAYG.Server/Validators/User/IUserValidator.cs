using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Validators.User
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserValidator
    {
        Task<bool> ValidateUserExists(string userName);
    }
}
