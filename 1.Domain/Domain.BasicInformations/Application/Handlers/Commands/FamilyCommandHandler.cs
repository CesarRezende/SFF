using SFF.Domain.BasicInformations.Application.Commands;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Repository;

namespace SFF.Domain.BasicInformations.Application.Handlers.Commands
{
    public class FamilyCommandHandler : BaseCommandHandler,
        ICommandHandler<InsertFamilyCommand>,
        ICommandHandler<UpdateFamilyCommand>,
        ICommandHandler<DeleteFamilyCommand>
    {

        private readonly IBasicInformationsAppService _basicInformationAppService;
        private readonly IUnitOfWork _unitOfWork;

        public FamilyCommandHandler( IBasicInformationsAppService basicInformationAppService
            , IUnitOfWork unitOfWork)
        {
            _basicInformationAppService = basicInformationAppService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommandResult> Execute(InsertFamilyCommand command)
        {
            return await _unitOfWork.RunAsync<CommandResult>( async () => {

                return await _basicInformationAppService.InsertFamilyAsync(command.Description);

            });
        }
        public async Task<CommandResult> Execute(UpdateFamilyCommand command)
        {
            return await _unitOfWork.RunAsync<CommandResult>(async () => {
                return await _basicInformationAppService.UpdateFamilyAsync(command.Id , command.Description);
            });
        }
        public async Task<CommandResult> Execute(DeleteFamilyCommand command)
        {
            return await _unitOfWork.RunAsync<CommandResult>(async () => {

                return Result.Success();

            });
        }


    }
}
