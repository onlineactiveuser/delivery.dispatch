using Application.Interfaces;
using Data.Context;
using Data.Interfaces.PostgreDb;
using Data.Repositories.PostgreDb;
using Domain.Abstraction.Events;
using Domain.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Crosscutting.Services
{
    public class UnitOfWorkService : IUnitOfWorkService
    {
        private readonly IEventBusService _eventBusService;
        private readonly PostgreDbContext _dbContext;
        public IVehicleRepository Vehicles { get; private set; }

        public UnitOfWorkService(
            PostgreDbContext dbContext,
            IVehicleRepository vehicles,
            IEventBusService eventBusService)
        {
            Vehicles = new VehicleRepository(dbContext);
            _dbContext = dbContext;
            _eventBusService = eventBusService;
        }

        public async Task<bool> Commit()
        {

            await PublishEvents();
            await _dbContext.SaveChangesAsync();
            return true;
        }

        private async Task PublishEvents()
        {
            List<EntityEntry<AggregateRoot>> aggregateRoots = _dbContext.ChangeTracker
                   .Entries<AggregateRoot>()
                   .Where(entityEntry => entityEntry.Entity.Events.Any())
                   .ToList();

            List<IEvent> domainEvents = aggregateRoots
                .SelectMany(entityEntry => entityEntry.Entity.Events)
                .ToList();

            aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearEvents());

            IEnumerable<Task> tasks = domainEvents.Select(domainEvent => _eventBusService.Publish(domainEvent));

            await Task.WhenAll(tasks);
        }

    }
}
