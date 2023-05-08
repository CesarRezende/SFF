using Flunt.Notifications;
using Newtonsoft.Json;
using SFF.Infra.Core.DomainEvents;

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
        private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();

        [JsonIgnore]
        public ICollection<IDomainEvent> DomainEvents => _domainEvents;

        public void AddEvent(IDomainEvent @event)
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event), "Invalid domain event");
            _domainEvents.Add(@event);
        }

        public void RemoveEvent(IDomainEvent @event)
        {
            if (@event is null) throw new ArgumentNullException(nameof(@event), "Invalid domain event");
            _domainEvents.Remove(@event);
        }
        public void ClearEvents() {
            _domainEvents.Clear();
        }
    }
}