using PAYG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PAYG.Domain.RepositoryInterfaces
{
    public interface IVehiclesRepository
    {
        Task<List<Vehicle>> GetList(int userId);

        Task<Vehicle> Get(int vehicleId);

        Task<Vehicle> Add(Vehicle vehicle, int userId);

        Task Update(Vehicle vehicle, int userId);

        Task Delete(int vehicleId, int userId);
    }
}
