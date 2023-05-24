using Flunt.Validations;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.Validations.Interface;
using SFF.Infra.Core.Validations.Models;

namespace SFF.Domain.Administration.Application.Commands
{
    public class GeneratePassawordCommand : CommandBase
    {
        public string PlainPassword { get; set; }
    }


    public class GeneratePassawordCommandValidator : IValidator<GeneratePassawordCommand>
    {
        public ValidationResult Validate(GeneratePassawordCommand instance)
        {
            var result = new ValidationResult();
            result.AddNotifications(new Contract<GeneratePassawordCommand>().Requires()
                .IsNotNullOrEmpty(instance.PlainPassword, "GeneratePassawordCommand.PlainPassword", "Senha é obrigatório")
                .IsGreaterOrEqualsThan(instance.PlainPassword.Length, 6, "GeneratePassawordCommand.PlainPassword", "Senha deve conter ao menos 6 caracteres")
                );

            return result;
        }
    }
}
