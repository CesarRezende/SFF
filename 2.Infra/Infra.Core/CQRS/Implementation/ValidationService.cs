using DryIoc;
using Flunt.Notifications;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.IoC;
using System.Diagnostics;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public class FluntValidationService : IValidationService
    {
        public IReadOnlyCollection<Notification> Validate<T>(T entity) where T : ICommand
        {
            try
            {
                var container = ContainerManager.GetContainer();

                if (!container.IsRegistered<IValidator<T>>())
                    return null;

                var validator = container.Resolve<IValidator<T>>();

                return validator?.Validate(entity);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            return null;
        }

        public IReadOnlyCollection<Notification> ValidateQuery<T>(T entity) where T : IQuery
        {
            try
            {
                var container = ContainerManager.GetContainer();

                if (!container.IsRegistered<IValidator<T>>())
                    return null;

                var validator = container.Resolve<IValidator<T>>();

                return validator.Validate(entity);
            }
            catch (Exception ex)
            {
                Debug.Write(ex);
            }

            return null;
        }
    }
}
