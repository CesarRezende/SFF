using SFF.Infra.Core.CQRS.Interfaces;
using System.Text.Json.Serialization;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public abstract class CommandBase : ICommand
    {
        [JsonIgnore]
        public Guid UId { get; set; } = Guid.NewGuid();
    }
}
