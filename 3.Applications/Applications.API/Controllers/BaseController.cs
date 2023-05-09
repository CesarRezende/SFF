using AutoMapper;
using Flunt.Notifications;
using Infra.IoC;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.SharedKernel.Helpers;
using DryIoc;

namespace SFF.Applications.API.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public abstract class BaseController : Controller
    {

        protected readonly ICommandDispatcher CommandDispatcher;
        protected readonly IQueryDispatcher QueryDispatcher;
        private readonly Func<IValidationDictionary> _modelStateResolver;
        protected readonly IMapper _mapper;
        protected readonly ILogger<BaseController> _logger;
        public BaseController(IMapper mapper, ILogger<BaseController> logger)
        {
            var container = ContainerManager.GetContainer();

            QueryDispatcher = container.Resolve<IQueryDispatcher>();
            CommandDispatcher = container.Resolve<ICommandDispatcher>();

            _mapper = mapper != null ? mapper : throw new ArgumentNullException("mapper");
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
                : (IActionResult)BadRequest(commandResult);
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
