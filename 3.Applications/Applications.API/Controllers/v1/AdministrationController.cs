using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFF.Domain.Administration.Application.Commands;
using SFF.Domain.Administration.Application.Queriables.Queries;
using SFF.Domain.Administration.Application.Queriables.QueryResult;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Applications.API.Controllers.v1
{
    [ApiController]
    [Route("v1/administration")]
    public class AdministrationController : BaseController
    {


        public AdministrationController(ILogger<AdministrationController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            :base(logger, queryDispatcher, commandDispatcher)
        {

        }



        [HttpGet]
        [Route("user/all")]
        public async Task<IEnumerable<UserQueryResult>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            return await QueryDispatcher.Dispatch<GetAllUsersQuery, IEnumerable<UserQueryResult>>(query);
        }


        [HttpPost]
        [Route("auth/generate-password")]
        public async Task<IActionResult> InsertFamily([FromBody] GeneratePassawordCommand command)
        {
            return Response(await CommandDispatcher.Dispatch(command));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("auth/authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateCommand command)
        {
            command.Ip = Request.HttpContext.Connection.RemoteIpAddress.ToString();

            return Response(await CommandDispatcher.Dispatch(command));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("auth/refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            return Response(await CommandDispatcher.Dispatch(command));
        }
    }
}