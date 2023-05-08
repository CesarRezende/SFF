using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace SFF.Infra.Core.DomainEvents
{
    public class DomainEventsDispatcher : IDomainEventDispatcher
    {
        private static IServiceProvider _serviceProvider;
        private static ILogger _logger;

        public DomainEventsDispatcher(IServiceProvider serviceProvider, ILogger<DomainEventsDispatcher> logger)
        {
            _serviceProvider = serviceProvider != null ? serviceProvider : throw new ArgumentNullException("ServiceProvider");
            _logger = logger != null ? logger : throw new ArgumentNullException("Logger");
        }

        public async Task Dispatch<T>(T @event) where T : IDomainEvent
        {
            var eventName = @event.GetType().Name;

            _logger.LogInformation("Raising event {eventName}", eventName);
            var handlers = _serviceProvider.GetServices<IDomainEventHandler<T>>();
            _logger.LogDebug("Founded {numOfHandlers} handlers to Event {eventName}", handlers.Count(), eventName);

            foreach (var handler in handlers)
            {
                var handlerName = handler.GetType().Name;
                _logger.LogDebug("Calling Handler {handlerName}", handlerName);
                await handler.Handle(@event);
            }
        }

        public Task DispatchAll<T>(IEnumerable<T> events) where T : IDomainEvent
        {
            return Task.Run(async () => {

                foreach (var ev in events)
                {
                    var type = ev.GetType();

                    MethodInfo method = typeof(DomainEventsDispatcher).GetMethod(nameof(DomainEventsDispatcher.Dispatch));
                    MethodInfo genericMethod = method.MakeGenericMethod(type);
                    dynamic task = genericMethod.Invoke(this, new object[] { ev });

                    await task;
                }
            });
        }
    }
}