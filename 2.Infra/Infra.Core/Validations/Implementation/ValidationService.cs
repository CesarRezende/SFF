using DryIoc;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Validations.Interface;
using SFF.Infra.Core.Validations.Models;
using SFF.Infra.IoC;
using System.Diagnostics;

namespace SFF.Infra.Core.Validations.Implementation
{
    public class FluntValidationService : IValidationService
    {
        public ValidationResult Validate<T>(T entity) where T : ICommand
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

        public ValidationResult ValidateQuery<T>(T entity) where T : IQuery
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
