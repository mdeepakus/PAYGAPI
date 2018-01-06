using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Entities
{
    public class RegisterNewUser
    {
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

    }
}
