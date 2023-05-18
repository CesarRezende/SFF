using SFF.Infra.IoC;
using SFF.Infra.Core.CQRS.Interfaces;
using DryIoc;
using SFF.Infra.Core.Validations.Interface;

namespace SFF.Infra.Core.CQRS.Implementation
{

    public abstract class BaseEventHandler
    {
        public virtual int HandlerOrder { get; set; }

        public BaseEventHandler()
        {
        }

        /// <summary>
        /// Return the CommandDispatch instance
        /// </summary>
        protected ICommandDispatcher CommandDispatcher => ContainerManager.GetContainer().Resolve<ICommandDispatcher>();

        protected IValidationDictionary ModelState => ContainerManager.GetContainer().Resolve<IValidationDictionary>();

        protected IEventDispatcher EventDispatcher => ContainerManager.GetContainer().Resolve<IEventDispatcher>();

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
                if (!ModelState.IsValid) return CommandResult.Invalid();
            }

            return await Task.FromResult(CommandResult.Valid());
        }
    }
}
