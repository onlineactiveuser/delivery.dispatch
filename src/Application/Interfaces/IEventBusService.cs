using Application.Abstraction.Messaging;
using Domain.Abstraction.Messaging;

namespace Application.Interfaces
{
    public interface IEventBusService
    {
        Task Publish<T>(T @event) where T : IEvent;
        void Subscribe<E, EH>(CancellationToken cancellationToken = default)
            where E : IEvent
            where EH : IEventHandler<E>;
    }
}
