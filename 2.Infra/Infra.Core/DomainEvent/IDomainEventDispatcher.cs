namespace SFF.Infra.Core.DomainEvents
{
    public interface IDomainEventDispatcher
    {
        Task Dispatch<T>(T @event) where T : IDomainEvent;

        Task DispatchAll<T>(IEnumerable<T> events) where T : IDomainEvent;
    }
}
