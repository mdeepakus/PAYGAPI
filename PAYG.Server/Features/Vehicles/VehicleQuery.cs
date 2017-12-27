using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PAYG.Server.Features.Vehicles
{
    public abstract class VehicleQuery
    {
        /// <summary>
        /// VehicleID
        /// </summary>
        public int VehicleID { get; set; }
    }
}
