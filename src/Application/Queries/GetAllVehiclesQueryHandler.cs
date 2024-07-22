using Application.Abstraction.Messaging;
using Data.Interfaces.MongoDb;
using Domain.Events;
using Microsoft.Extensions.Logging;

namespace Application.Queries
{
    public class GetAllVehiclesQueryHandler : IQueryHandler<GetAllVehiclesQuery, IEnumerable<VehicleCreatedEvent>>
    {
        private readonly IMongoDbContext<VehicleCreatedEvent> _mongoDbContext;
        private readonly ILogger<GetAllVehiclesQueryHandler> _logger;
        public GetAllVehiclesQueryHandler(
            IMongoDbContext<VehicleCreatedEvent> mongoDbContext,
            ILogger<GetAllVehiclesQueryHandler> logger)
        {
            _mongoDbContext = mongoDbContext;
            _logger = logger;
        }
        public async Task<IEnumerable<VehicleCreatedEvent>> Handle(GetAllVehiclesQuery query, CancellationToken cancellationToken)
        {
            try
            {
                return await _mongoDbContext.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Message: {0} StackTrace: {1}", ex.Message, ex.StackTrace);
                throw;
            }

        }
    }
}
