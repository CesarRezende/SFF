using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFF.Domain.BasicInformations.Application.Commands;
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


        #region Family
        [HttpGet]
        [AllowAnonymous]
        [Route("family/all")]
        public async Task<IEnumerable<FamilyQueryResult>> GetAllFamilies()
        {
            var query = new GetAllFamiliesQuery();
            return await QueryDispatcher.Dispatch<GetAllFamiliesQuery, IEnumerable<FamilyQueryResult>>(query);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("family")]
        public async Task<IActionResult> InsertFamily([FromBody] InsertFamilyCommand command)
        {
            return Response(await CommandDispatcher.Dispatch(command));
        }

        [HttpPut]
        [AllowAnonymous]
        [Route("family")]
        public async Task<IActionResult> UptadeFamily([FromBody] UpdateFamilyCommand command)
        {
            return Response(await CommandDispatcher.Dispatch(command));
        }

        [HttpDelete]
        [AllowAnonymous]
        [Route("family")]
        public async Task<IActionResult> UptadeFamily([FromBody] DeleteFamilyCommand command)
        {
            return Response(await CommandDispatcher.Dispatch(command));
        }

        #endregion Family
    }
}