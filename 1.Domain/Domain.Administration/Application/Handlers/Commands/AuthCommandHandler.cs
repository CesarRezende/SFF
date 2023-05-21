
using SFF.Domain.Administration.Application.Commands;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Repository;


namespace SFF.Domain.Administration.Application.Handlers.Commands
{
    public class AuthCommandHandler : BaseCommandHandler,
        ICommandHandler<GeneratePassawordCommand>,
        ICommandHandler<AuthenticateCommand>
    {

        private readonly IAdministrationAppService _administrationAppService;
        private readonly IUnitOfWork _unitOfWork;

        public AuthCommandHandler(IAdministrationAppService administrationAppService
            , IUnitOfWork unitOfWork)
        {
            _administrationAppService = administrationAppService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Execute(GeneratePassawordCommand command)
        {
            return await _administrationAppService.GeneratePasswordAsync(command.PlainPassword);
        }

        public async Task<CommandResult> Execute(AuthenticateCommand command)
        {
            return await _unitOfWork.RunAsync<CommandResult>(async () => {

                return await _administrationAppService.Authenticate(command.Login, command.Password);

            });
            
        }

    }
}
