using SFF.Domain.Administration.Application.Queriables.Queries;
using SFF.Domain.Administration.Application.Queriables.QueryResult;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.Administration.Application.Handlers.Queries
{
    public class UserQueryHandler :
        IQueryHandler<GetAllUserQuery, IEnumerable<UserQueryResult>>
    {
        private readonly IAdministrationAppService _administrationAppService;
        public UserQueryHandler(IAdministrationAppService administrationAppService)
        {
            _administrationAppService = administrationAppService;
        }

        public Task<IEnumerable<UserQueryResult>> Retrieve(GetAllUserQuery query)
        {
            return _administrationAppService.Query.GetAll();
        }
    }
}
