using Flunt.Validations;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.Validations.Interface;
using SFF.Infra.Core.Validations.Models;

namespace SFF.Domain.Administration.Application.Commands
{
    public class AuthenticateCommand : CommandBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }


    public class AuthenticateCommandValidator : IValidator<AuthenticateCommand>
    {
        public ValidationResult Validate(AuthenticateCommand instance)
        {
            var result = new ValidationResult();
            result.AddNotifications(new Contract<AuthenticateCommand>().Requires()
                .IsNotNullOrEmpty(instance.Login, "AuthenticateCommand.Login", "Login é obrigatório")
                .IsGreaterOrEqualsThan(instance.Login.Length, 3, "AuthenticateCommand.Login", "Login deve conter ao menos 3 caracteres")

                .IsNotNullOrEmpty(instance.Password, "AuthenticateCommand.Password", "Senha é obrigatória")
                .IsGreaterOrEqualsThan(instance.Password.Length, 6, "AuthenticateCommand.Password", "Senha deve conter ao menos 6 caracteres")
                );

            return result;
        }
    }
}
