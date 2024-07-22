using Application.EventHandlers;
using Application.Interfaces;
using Domain.Events;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.BackgroundWorker
{
    public class BackgroundWorkerService : BackgroundService
    {
        private readonly ILogger<BackgroundWorkerService> _logger;
        private readonly IEventBusService _eventBus;
        public BackgroundWorkerService(ILogger<BackgroundWorkerService> logger, IEventBusService eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _eventBus.Subscribe<VehicleCreatedEvent, VehicleCreatedEventHandler>();
            return Task.CompletedTask;
        }

    }
}
