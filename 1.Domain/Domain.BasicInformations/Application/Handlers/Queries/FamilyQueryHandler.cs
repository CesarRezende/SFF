using SFF.Domain.BasicInformations.Application.Queriables.Queries;
using SFF.Domain.BasicInformations.Application.Queriables.QueryResult;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.BasicInformations.Application.Handlers.Queries
{
    public class FamilyQueryHandler :
        IQueryHandler<GetAllFamiliesQuery, IEnumerable<FamilyQueryResult>>
    {
        private readonly IBasicInformationsAppService _basicInformationAppService;
        public FamilyQueryHandler(IBasicInformationsAppService basicInformationAppService)
        {
            _basicInformationAppService = basicInformationAppService;
        }

        public Task<IEnumerable<FamilyQueryResult>> Retrieve(GetAllFamiliesQuery query)
        {
            return _basicInformationAppService.Query.GetAll();
        }
    }
}
