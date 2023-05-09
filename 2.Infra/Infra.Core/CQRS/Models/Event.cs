
namespace SFF.Infra.Core.CQRS.Models
{
    public abstract class Event : Message
    {
        public Guid Id { get; private set; }
        public DateTime Timestamp { get; private set; }
        public string Type => GetType().Name;

        protected Event(Guid uniqueIdentifier)
        {
            Id = Guid.NewGuid();
            base.AggregateId = uniqueIdentifier;
            Timestamp = DateTime.Now;
        }
    }
}
