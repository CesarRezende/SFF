using SFF.Infra.Core.Validations.Interface;

namespace SFF.Infra.Core.CQRS.Interfaces
{
    public interface ICommandValidator<T> where T : ICommand
    {
        /// <summary>
        /// Execute a validation on the Command and set erros on ModelState
        /// </summary>
        /// <param name="modelState">The validation dictionary to be validated</param>
        /// <returns>Return if Validation has errors</returns>
        bool Validate(T command, IValidationDictionary modelState);
    }
}
