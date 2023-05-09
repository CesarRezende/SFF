namespace SFF.Infra.Core.CQRS.Interfaces
{
    public interface IDynamicEventHandler
    {
        Task Handle(dynamic @event);
    }
}
