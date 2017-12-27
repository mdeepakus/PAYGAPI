using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Entities
{
    public class ApplicationUser
    {
         public ApplicationUser(int userId, string username, string password = "")
        {
            UserId = userId;
            Username = username;
            Password = password;
        }

        public int UserId { get; private set; }

        public string Username { get; private set; }
        
        public string Password { get; private set; }

    }
}

