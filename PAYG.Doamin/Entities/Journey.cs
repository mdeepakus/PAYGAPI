using System;
using System.Collections.Generic;
using System.Text;

namespace PAYG.Domain.Entities
{
    public class Journey
    {
        public int Id { get; set; }
        public int VehicleId { get; set; }
        public int UserId { get; set; }
        public DateTime? StarteDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
