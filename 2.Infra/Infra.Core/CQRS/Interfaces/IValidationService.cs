using Flunt.Notifications;

namespace SFF.Infra.Core.CQRS.Interfaces
{
    public interface IValidationService
    {
        IReadOnlyCollection<Notification> Validate<T>(T entity) where T : ICommand;
        IReadOnlyCollection<Notification> ValidateQuery<T>(T entity) where T : IQuery;
    }
}
