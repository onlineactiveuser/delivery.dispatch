using Microsoft.Extensions.Logging;

namespace Domain.Abstraction.Messaging
{
    public interface IEventHandler<in TEvent> : IEventHandler
       where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
    public interface IEventHandler
    {
    }
}