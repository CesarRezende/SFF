using DryIoc;
using Microsoft.Extensions.Logging;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IContainer _container;
        private readonly ILogger<QueryDispatcher> _logger;
        private readonly Func<IValidationDictionary> _validationDictionary;
        private readonly IValidationService _validationService;

        public QueryDispatcher
        (
            Func<IValidationDictionary> validationDictionary,
            IContainer container,
            ILogger<QueryDispatcher> logger,
            IValidationService validationService
        )
        {
            _container = container;
            _logger = logger;
            _validationDictionary = validationDictionary;
            _validationService = validationService;
        }

        public async Task<TResult> Dispatch<TParameter, TResult>(TParameter query) where TParameter : IQuery
        {
            var modelState = _validationDictionary();
            var handlerName = default(string);
            var queryName = default(string);

            queryName = query?.GetType()?.Name;

            try
            {

                var handler = _container.Resolve<IQueryHandler<TParameter, TResult>>();

                if (handler == null)
                {
                    _logger?.LogError($"Não foi encontrado nenhum QueryHanddler para o tipo {queryName}");
                    return default;
                }

                handlerName = handler.GetType().Name;

                _logger?.LogInformation($"Foi encontrado o handler {handlerName} para o tipo {queryName}");

                if (_validationService != null)
                    modelState.AddModelError(_validationService.ValidateQuery(query));

                if (!modelState.IsValid)
                {
                    return default;
                }

                _logger?.LogDebug($"Executando handler {handlerName} para o tipo {queryName}");
                var result = await handler.Retrieve(query);
                _logger?.LogDebug($"Executando handler {handlerName} para o tipo {queryName} com sucesso!");

                return result;
            }
            catch (Exception ex)
            {

                _logger?.LogError(ex, $"Ocorreu um erro inesperado ao executar QueryHanddler para o tipo {queryName}");

                modelState.AddModelError("Ocorreu um erro inesperado ao executar a requisição");

                throw ex.InnerException ?? ex;
            }
            finally
            {
            }
        }
    }
}
