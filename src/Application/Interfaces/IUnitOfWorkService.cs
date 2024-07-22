using Data.Interfaces.PostgreDb;

namespace Application.Interfaces
{
    public interface IUnitOfWorkService
    {
        IVehicleRepository Vehicles { get; }
        Task<bool> Commit();
    }
}
