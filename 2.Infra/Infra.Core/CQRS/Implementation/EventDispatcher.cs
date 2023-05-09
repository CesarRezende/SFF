using DryIoc;
using Microsoft.Extensions.Logging;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.CQRS.Models;
using SFF.Infra.Core.Repository;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IContainer _container;
        private readonly ILogger _logger;

        public EventDispatcher
        (
            IContainer container,
            ILogger logger
        )
        {
            _container = container;
            _logger = logger;
        }

        public async Task Dispatch<TEvent>(TEvent @event) where TEvent : Event
        {
            IEventHandler<TEvent> handler = null;
            try
            {
                var handlerList = _container.ResolveMany<IEventHandler<TEvent>>()?.ToList();

                handlerList = handlerList.OrderBy(x => (x as BaseEventHandler)?.HandlerOrder ?? 0).ToList();

                if (handlerList == null) return;

                for (var i = 0; i < handlerList.Count; i++)
                {
                    handler = handlerList[i];
                    await handler.Execute(@event);
                }

            }
            catch (Exception ex)
            {
                var unitOfWork = _container.Resolve<IUnitOfWork>();

                if (unitOfWork != null)
                {
                    unitOfWork.Rollback();
                }

                var unitOfWorkWithTransactionScope = _container.Resolve<IUnitOfWorkWithTransactionScope>();

                if (unitOfWorkWithTransactionScope != null)
                {
                    unitOfWorkWithTransactionScope.Rollback();
                }


                if (_logger != null)
                {
                    _logger.LogError(ex, "Ocorreu um erro inesperado");
                }

                throw ex.InnerException ?? ex;
            }
        }
    }
}
