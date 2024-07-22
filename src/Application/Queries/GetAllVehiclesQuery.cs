using Application.Abstraction.Messaging;
using Domain.Events;

namespace Application.Queries
{
    public class GetAllVehiclesQuery : IQuery<IEnumerable<VehicleCreatedEvent>>
    {
    }
}
