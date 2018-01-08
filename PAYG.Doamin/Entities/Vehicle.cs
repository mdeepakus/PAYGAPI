using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Entities
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Type { get; set; }
    }
}
