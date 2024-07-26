using Microsoft.Extensions.Logging;

namespace Domain.Abstraction.Events
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