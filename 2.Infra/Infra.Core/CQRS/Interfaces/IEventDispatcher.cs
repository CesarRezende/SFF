using SFF.Infra.Core.CQRS.Models;

namespace SFF.Infra.Core.CQRS.Interfaces
{
    public class EnvelopedMessage<TEvent> where TEvent : Event
    {
        public EnvelopedMessage(TEvent @event)
        {
            Id = @event.AggregateId;
            Data = @event;
        }

        public Guid Id { get; internal set; }

        public object Data { get; set; }
    }

    public interface IEventStore
    {
        Task Send<TEvent>(EnvelopedMessage<TEvent> @event) where TEvent : Event;
    }

    public interface IEventHandler<in TEvent> where TEvent : Event
    {
        Task Execute(TEvent @event);
    }

    public interface IEventDispatcher
    {
        Task Dispatch<TEvent>(TEvent @event) where TEvent : Event;
    }
}
