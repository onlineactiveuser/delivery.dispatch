using Data.Context;
using Data.Interfaces.PostgreDb;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.PostgreDb
{
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        public VehicleRepository(PostgreDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistVehicleByPlate(string plate)
        {
            return await _dbContext.Set<Vehicle>().Where(x=> x.Plate == plate).AnyAsync();
        }

    }
}
