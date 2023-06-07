using SFF.Infra.Core.CQRS.Models;
using SFF.Infra.Core.Security.Models;

namespace SFF.Domain.Administration.Application.Events
{
    public class UserAuthenticatedEvent : Event
    {
        public UserAuthenticatedEvent(string ip, long userId, AuthInformation authInformation) : base(Guid.NewGuid())
        {
            IP = ip;
            UserID = userId;
            AuthInformation = authInformation;
        }

        public string IP { get; private set; }
        public long UserID { get; private set; }
        public AuthInformation AuthInformation { get; private set; }
    }
}
