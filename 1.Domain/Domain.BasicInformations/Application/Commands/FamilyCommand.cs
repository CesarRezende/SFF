using Newtonsoft.Json;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.BasicInformations.Application.Commands
{
    public class BaseFamilyCommand : ICommand
    {
        [System.Text.Json.Serialization.JsonIgnore]
        [JsonIgnore]
        public Guid UId { get; set; } = Guid.NewGuid();

        public string Description { get; set; }

    }

    public class InsertFamilyCommand : BaseFamilyCommand
    {

    }

    public class UpdateFamilyCommand : BaseFamilyCommand
    {
        public long Id { get; set; }
    }


    public class DeleteFamilyCommand : ICommand
    {
        public Guid UId { get; set; } = Guid.NewGuid();
        public long Id { get; set; }
    }

}
