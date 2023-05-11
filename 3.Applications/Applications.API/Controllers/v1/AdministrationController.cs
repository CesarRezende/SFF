using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFF.Domain.Administration.Application.Queriables.Queries;
using SFF.Domain.Administration.Application.Queriables.QueryResult;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Applications.API.Controllers.v1
{
    [ApiController]
    [Route("v1/[controller]")]
    public class AdministrationController : BaseController
    {


        public AdministrationController(ILogger<AdministrationController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            :base(logger, queryDispatcher, commandDispatcher)
        {

        }


        [HttpGet]
        [AllowAnonymous]
        [Route("all")]
        public async Task<IEnumerable<UserQueryResult>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            return await QueryDispatcher.Dispatch<GetAllUsersQuery, IEnumerable<UserQueryResult>>(query);
        }

    }
}