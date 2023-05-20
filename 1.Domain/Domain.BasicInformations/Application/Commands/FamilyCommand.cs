using SFF.Infra.Core.CQRS.Implementation;

namespace SFF.Domain.BasicInformations.Application.Commands
{
    public class BaseFamilyCommand : CommandBase
    {

        public string Description { get; set; }

    }

    public class InsertFamilyCommand : BaseFamilyCommand
    {

    }

    public class UpdateFamilyCommand : BaseFamilyCommand
    {
        public long Id { get; set; }
    }


    public class DeleteFamilyCommand : CommandBase
    {
        public long Id { get; set; }
    }

}
