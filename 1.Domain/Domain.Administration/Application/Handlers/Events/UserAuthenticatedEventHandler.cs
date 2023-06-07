using SFF.Domain.Administration.Application.Events;
using SFF.Infra.Core.CQRS.Implementation;
using SFF.Infra.Core.CQRS.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SFF.Domain.Administration.Application.Handlers.Events
{
    public class UserAuthenticatedEventHandler : BaseEventHandler, IEventHandler<UserAuthenticatedEvent>
    {
        private readonly IAdministrationAppService _administrationAppService;
        public UserAuthenticatedEventHandler(IAdministrationAppService administrationAppService)
        {
            _administrationAppService = administrationAppService;
        }

        public async Task Execute(UserAuthenticatedEvent @event)
        {
            var result = await _administrationAppService.CreateSession(@event.IP,@event.UserID, @event.AuthInformation);

            if (!result.Success)
                throw new ValidationException(result.Message);

        }
    }
}
