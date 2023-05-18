using Flunt.Notifications;

namespace SFF.Infra.Core.Validations.Interface
{
    public interface IValidator<in T>
    {
        IReadOnlyCollection<Notification> Validate(T instance);
        Task<IReadOnlyCollection<Notification>> ValidateAsync(T instance, CancellationToken cancellation = default);
    }
}
