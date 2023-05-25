using Flunt.Validations;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.Validations.Interface;
using SFF.Infra.Core.Validations.Models;

namespace SFF.Domain.Administration.Application.Commands
{
    public class RefreshTokenCommand : CommandBase
    {
        public string RefreshToken { get; set; }
    }


    public class RefreshTokenCommandValidator : IValidator<RefreshTokenCommand>
    {
        public ValidationResult Validate(RefreshTokenCommand instance)
        {
            var result = new ValidationResult();
            result.AddNotifications(new Contract<RefreshTokenCommand>().Requires()
                .IsNotNullOrEmpty(instance.RefreshToken, "RefreshTokenCommand.RefreshToken", "RefreshToken é obrigatório")
                .AreEquals(instance.RefreshToken.Length, 44, "RefreshTokenCommand.RefreshToken", "RefreshToken é invalido")
                );

            return result;
        }
    }
}
