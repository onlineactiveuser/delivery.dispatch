using Domain.Entities;

namespace Data.Interfaces.PostgreDb
{
    public interface IVehicleRepository : IGenericRepository<Vehicle>
    {
        Task<bool> ExistVehicleByPlate(string plate);
    }
}
