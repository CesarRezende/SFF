using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFF.Domain.BasicInformations.Application.Queriables.Queries;
using SFF.Domain.BasicInformations.Application.Queriables.QueryResult;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Applications.API.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class BasicInformationsController : BaseController
    {


        public BasicInformationsController(ILogger<BasicInformationsController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            :base(logger, queryDispatcher, commandDispatcher)
        {

        }


        [HttpGet]
        [AllowAnonymous]
        [Route("all")]
        public async Task<IEnumerable<FamilyQueryResult>> GetAllFamilys()
        {
            var query = new GetAllFamiliesQuery();
            return await QueryDispatcher.Dispatch<GetAllFamiliesQuery, IEnumerable<FamilyQueryResult>>(query);
        }

    }
}