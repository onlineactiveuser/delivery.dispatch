using Domain.Abstraction.Events;

namespace Domain.Entities
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

        public void ClearEvents() => _events.Clear();

        protected void AddEvent(IEvent domainEvent) => _events.Add(domainEvent);
    }
}
