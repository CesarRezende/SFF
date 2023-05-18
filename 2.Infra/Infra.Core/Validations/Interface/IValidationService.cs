using Flunt.Notifications;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Infra.Core.Validations.Interface
{
    public interface IValidationService
    {
        IReadOnlyCollection<Notification> Validate<T>(T entity) where T : ICommand;
        IReadOnlyCollection<Notification> ValidateQuery<T>(T entity) where T : IQuery;
    }
}
