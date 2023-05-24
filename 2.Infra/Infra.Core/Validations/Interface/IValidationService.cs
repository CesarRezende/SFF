using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Validations.Models;

namespace SFF.Infra.Core.Validations.Interface
{
    public interface IValidationService
    {
        ValidationResult Validate<T>(T entity) where T : ICommand;
        ValidationResult ValidateQuery<T>(T entity) where T : IQuery;
    }
}
