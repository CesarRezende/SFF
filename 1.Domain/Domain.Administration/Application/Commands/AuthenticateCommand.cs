using SFF.Infra.Core.CQRS.Implementation;

namespace SFF.Domain.Administration.Application.Commands
{
    public class AuthenticateCommand : CommandBase
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
