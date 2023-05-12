using Flunt.Notifications;
using Newtonsoft.Json;
using SFF.Infra.Core.CQRS.Models;

namespace SFF.Domain.SharedKernel
{
    public abstract class Entity<T> : Notifiable<Notification>
    {

        protected Entity(T id)
        {
            Id = id;
        }

        public T Id { get; set; }

        [JsonIgnore]
        private readonly List<Event> _domainEvents = new List<Event>();

        [JsonIgnore]
        public List<Event> DomainEvents => _domainEvents;

        public void AddEvent(Event @event)
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event), "Invalid domain event");
            _domainEvents.Add(@event);
        }

        public void RemoveEvent(Event @event)
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event), "Invalid domain event");
            _domainEvents.Remove(@event);
        }
        public void ClearEvents() {
            _domainEvents.Clear();
        }
    }
}