using Data.Interfaces.MongoDb;
using Domain.Abstraction.Messaging;
using Domain.Events;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.EventHandlers
{
    public class VehicleCreatedEventHandler : IEventHandler<VehicleCreatedEvent>
    {
        private readonly IMongoDbContext<VehicleCreatedEvent> _mongoDbContext;
        private readonly ILogger<VehicleCreatedEventHandler> _logger;
        public VehicleCreatedEventHandler(
            IMongoDbContext<VehicleCreatedEvent> dbContext, 
            ILogger<VehicleCreatedEventHandler> logger)
        {
            _mongoDbContext = dbContext;
            _logger = logger;
        }

        public async Task Handle(VehicleCreatedEvent @event)
        {
            try
            {
                _logger.LogInformation("Consuming event {0} Parameters : {1}", nameof(VehicleCreatedEvent), JsonSerializer.Serialize(@event));
                await _mongoDbContext.AddAsync(@event);
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} StackTrace: {1}", ex.Message, ex.StackTrace);
            }

        }
    }
}
