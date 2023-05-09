using DryIoc;
using Microsoft.Extensions.Logging;
using SFF.Infra.Core.CQRS.Interfaces;
using SFF.Infra.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IContainer _container;
        private readonly ILogger _logger;
        private readonly Func<IValidationDictionary> _validationDictionary;
        private readonly IValidationService _validationService;

        public CommandDispatcher
        (
            Func<IValidationDictionary> validationDictionary,
            IContainer container,
            ILogger logger,
            IValidationService validationService
        )
        {
            _container = container;
            _logger = logger;
            _validationDictionary = validationDictionary;
            _validationService = validationService;
        }

        public async Task<ICommandResult> Dispatch<TParameter>(TParameter command) where TParameter : ICommand
        {
            var modelState = _validationDictionary();
            var handlerName = default(string);
            var commanderName = default(string);

            try
            {

                var handler = _container.Resolve<ICommandHandler<TParameter>>();

                if (handler == null)
                    return CommandResult.AnyHandlerFound<TParameter>();

                handlerName = handler.GetType().Name;
                commanderName = command?.GetType()?.Name;


                if (_validationService != null)
                    modelState.AddModelError(_validationService.Validate(command));

                if (modelState.IsValid) return await handler.Execute(command);


                var isUnAuthorized = modelState.Errors.Any(x => x.Value.Any(v => v.Contains("Unauthorized")));

                return isUnAuthorized
                    ? CommandResult.InsufficientPrivileges()
                    : CommandResult.InvalidRequest(modelState.Errors);

            }
            catch (Exception ex)
            {
                var unitOfWork = _container.Resolve<IUnitOfWork>(IfUnresolved.ReturnDefault);

                unitOfWork?.Rollback();

                var unitOfWorkWithTransactionScope = _container.Resolve<IUnitOfWorkWithTransactionScope>();

                unitOfWorkWithTransactionScope?.Rollback();

                _logger.LogError(ex, "Ocorreu um erro inesperado!");

                modelState.AddModelError("Ocorreu um erro inesperado!");

                return CommandResult.InvalidRequest(modelState.Errors);
            }
            finally
            {

            }
        }

    }
}
