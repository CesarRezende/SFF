namespace SFF.Infra.Core.DomainEvents
{
    public interface IDomainEventHandler<TDomainEvent> where TDomainEvent : IDomainEvent
    {
        Task Handle(TDomainEvent @event);
    }
}