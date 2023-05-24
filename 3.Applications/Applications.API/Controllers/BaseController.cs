using Flunt.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Helper;

namespace SFF.Applications.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {

        protected readonly ICommandDispatcher CommandDispatcher;
        protected readonly IQueryDispatcher QueryDispatcher;
        protected readonly ILogger<BaseController> _logger;
        public BaseController(ILogger<BaseController> logger, IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
        {

            QueryDispatcher = queryDispatcher;
            CommandDispatcher = commandDispatcher;
            _logger = logger != null ? logger : throw new ArgumentNullException("logger");
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning($"Invalid request!");

                var result = new List<Notification>();
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        result.Add(new Notification(state.Key, error.ErrorMessage));
                    }
                }

                context.Result = BadRequest(result);

                _logger.LogWarning($"Errors: {result.ToJsonFormat()}");
            }

            base.OnActionExecuting(context);
        }


        protected IActionResult Response(ICommandResult commandResult)
        {
            if (commandResult == null)
                throw new ArgumentNullException("commandResult");

            return commandResult.Success
                ? Ok(commandResult.Data ?? (new { commandResult.Message, commandResult.Success }))
                : BadRequest(commandResult);
        }

        protected async Task<IActionResult> ResponseAsync(Task<ICommandResult> commandResult)
        {
            var result = await commandResult;

            if (result == null)
                throw new ArgumentNullException("commandResult");

            if (result.Success)
                return Ok(result.Data ?? new { Message = "Success" });
            else if (result.Message.Contains("Insufficient privileges"))
                return Unauthorized(result);
            else
                return BadRequest(result);
        }

    }
}
