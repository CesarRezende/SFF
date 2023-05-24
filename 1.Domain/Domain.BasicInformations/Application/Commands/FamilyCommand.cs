using Flunt.Validations;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.Validations.Interface;
using SFF.Infra.Core.Validations.Models;

namespace SFF.Domain.BasicInformations.Application.Commands
{
    public abstract class BaseFamilyCommand : CommandBase
    {
        public string Description { get; set; }

    }

    public class InsertFamilyCommand : BaseFamilyCommand
    {

    }

    public class InsertFamilyCommandValidator : IValidator<InsertFamilyCommand>
    {
        public ValidationResult Validate(InsertFamilyCommand instance)
        {
            var result = new ValidationResult();
            result.AddNotifications(new Contract<InsertFamilyCommand>().Requires()
                .IsNotNullOrEmpty(instance.Description, "InsertFamilyCommand.Description", "Descrição é obrigatório")
                .IsGreaterOrEqualsThan(instance.Description.Length, 3, "InsertFamilyCommand.Description", "Descrição deve conter ao menos 3 caracteres")
                );

            return result;
        }
    }

    public class UpdateFamilyCommand : BaseFamilyCommand
    {
        public long Id { get; set; }
    }


    public class UpdateFamilyCommandValidator : IValidator<UpdateFamilyCommand>
    {
        public ValidationResult Validate(UpdateFamilyCommand instance)
        {
            var result = new ValidationResult();
            result.AddNotifications(new Contract<UpdateFamilyCommand>().Requires()
                .IsNotNullOrEmpty(instance.Description, "UpdateFamilyCommand.Description", "Descrição é obrigatório")
                .IsGreaterOrEqualsThan(instance.Description.Length, 3, "UpdateFamilyCommand.Description", "Descrição deve conter ao menos 3 caracteres")
                );

            return result;
        }
    }

    public class DeleteFamilyCommand : CommandBase
    {
        public long Id { get; set; }
    }
}
