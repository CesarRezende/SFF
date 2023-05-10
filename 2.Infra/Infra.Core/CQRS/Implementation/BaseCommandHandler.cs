using Flunt.Notifications;
using SFF.Infra.IoC;
using SFF.Infra.Core.CQRS.Interfaces;
using DryIoc;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public class BaseCommandHandler
    {
        public BaseCommandHandler()
        {
        }

        /// <summary>
        /// Return the ModelState instance
        /// </summary>
        protected IValidationDictionary ModelState => ContainerManager.GetContainer().Resolve<IValidationDictionary>();

        /// <summary>
        /// Return the ValidationService instance
        /// </summary>
        protected IValidationService ValidationService => ContainerManager.GetContainer().Resolve<IValidationService>();

        /// <summary>
        /// Return the EventDispatcher instance
        /// </summary>
        protected IEventDispatcher EventDispatcher => ContainerManager.GetContainer().Resolve<IEventDispatcher>();

        /// <summary>
        /// Get the in-memory default CommandResults
        /// </summary>
        protected DefaultCommandResults Result { get; } = new DefaultCommandResults();

        /// <summary>
        /// Return the EventDispatcher result
        /// </summary>
        /// <param name="_events">Events</param>
        /// <returns></returns>
        protected async Task<ICommandResult> DispatchEvents(List<dynamic> _events)
        {
            for (var i = 0; i < _events.Count; i++)
            {
                var evento = _events[i];
                await EventDispatcher.Dispatch(evento);
                if (!ModelState.IsValid) return CommandResult.Invalid(ModelState.Errors);
            }

            return await Task.FromResult(CommandResult.Valid());
        }

        protected void AddErrorParaModelState(IReadOnlyList<Notification> validationResult)
        {
            if (!validationResult.Any())
            {
                foreach (var erro in validationResult)
                {
                    ModelState.AddModelError(erro.Message);
                }
            }
        }
    }
}
