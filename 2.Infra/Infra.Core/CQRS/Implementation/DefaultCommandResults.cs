using SFF.Infra.IoC;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Infra.Core.CQRS.Implementation
{
    public class DefaultCommandResults
    {
        public class ErrorMessages
        {
            public const string InserirRegistro = "Erro ao inserir o registro";
            public const string ExcluirRegistro = "Erro ao excluir o registro";
            public const string EditarRegistro = "Erro ao editar o registro";
            public const string NotificacaoAgenda = "Problema ao enviar notificação de agendamentos";
        }

        public Task<CommandResult> InsufficientPrivilegesAsync()
        {
            return Task.FromResult(InsufficientPrivileges());
        }

        public CommandResult InsufficientPrivileges()
        {
            return new CommandResult()
            {
                Success = false,
                Message = "Insufficient privileges"
            };
        }

        public Task<CommandResult> ExistentEntityAsync(string entityName)
        {
            return Task.FromResult(ExistentEntity(entityName));
        }

        public CommandResult ExistentEntity(string entityName)
        {
            return new CommandResult()
            {
                Success = false,
                Message = $"The '{entityName}' already exists"
            };
        }

        public Task<CommandResult> FromModelStateAsync(IValidationDictionary validationDictionary)
        {
            return Task.FromResult(FromModelState(validationDictionary));
        }

        public Task<CommandResult> FromModelStateAsync(string key, string message)
        {
            var validation = ContainerManager.GetContainer().Resolve(typeof(IValidationDictionary), DryIoc.IfUnresolved.ReturnDefault) as IValidationDictionary;
            validation.AddModelError(key, message);
            return FailedAsync(validation);
        }


        public CommandResult FromModelState(IValidationDictionary validationDictionary)
        {
            if (validationDictionary.IsValid)
                return Success();
            else
                return Failed(validationDictionary);
        }

        public Task<CommandResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public CommandResult Success()
        {
            return new CommandResult()
            {
                Success = true,
                Message = $"Command successfully executed",
            };
        }

        public Task<CommandResult> SuccessAsync(object data)
        {
            return Task.FromResult(Success(data));
        }

        public CommandResult Success(object data)
        {
            return new CommandResult()
            {
                Success = true,
                Message = $"Command successfully executed",
                Data = data
            };
        }

        public Task<CommandResult> FailedAsync(IValidationDictionary validationDictionary)
        {
            return Task.FromResult(Failed(validationDictionary));
        }

        public CommandResult Failed(IValidationDictionary validationDictionary)
        {
            return new CommandResult()
            {
                Success = false,
                Message = $"Invalid request",
                Data = validationDictionary.Errors
            };
        }

        public CommandResult Failed(string error)
        {
            return new CommandResult()
            {
                Success = false,
                Message = error
            };
        }

        public Task<CommandResult> FailedAsync(string error)
        {
            return Task.FromResult(Failed(error));
        }
    }
}
