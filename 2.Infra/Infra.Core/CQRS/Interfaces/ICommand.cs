using SFF.Infra.Core.CQRS.Models;
using System.Text.Json.Serialization;

namespace SFF.Infra.Core.CQRS.Interfaces
{
    public interface ICommand
    {
        [JsonIgnore]
        Guid UId { get; }
    }

    public interface ICommandResult
    {
        bool Success { get; }
        string Message { get; set; }
        object Data { get; set; }
    }

    public interface ICommandResult<TResult> : ICommandResult
    {
        new TResult Data { get; set; }
    }

    public interface ICommandHandler<in TParameter> where TParameter : ICommand
    {
        Task<CommandResult> Execute(TParameter command);
    }

    public interface ICommandHandler<in TParameter, TResult> where TParameter : ICommand where TResult : ICommandResult
    {
        Task<TResult> Execute(TParameter command);
    }

    public class CommandResult : ICommandResult
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        #region Default Results 

        private static readonly CommandResult InsufficientPrivilegesCommandResult = new CommandResult() { Success = false, Message = "Insufficient privileges" };
        public static CommandResult InsufficientPrivileges()
        {
            return InsufficientPrivilegesCommandResult;
        }

        public static CommandResult AnyHandlerFound<TCommand>()
        {
            return new CommandResult()
            {
                Success = false,
                Message = "Any handler found for " + typeof(TCommand).Name
            };
        }
        public static CommandResult InvalidRequest(IDictionary<string, string[]> modelStateErrors)
        {
            return new CommandResult()
            {
                Success = false,
                Message = "Invalid request",
                Data = modelStateErrors
            };
        }

        public static CommandResult Invalid()
        {
            return new CommandResult()
            {
                Success = false,
                Message = "Invalid command"
            };
        }

        public static CommandResult Invalid(string message)
        {
            return new CommandResult
            {
                Message = message,
                Success = false
            };
        }

        public static CommandResult Invalid(IDictionary<string, string[]> modelStateErrors)
        {
            return new CommandResult
            {
                Data = modelStateErrors,
                Success = false
            };
        }

        public static CommandResult Valid()
        {
            return new CommandResult
            {
                Success = true
            };
        }

        public static CommandResult Valid(object data)
        {
            return new CommandResult
            {
                Success = true,
                Data = data
            };
        }


        #endregion

        public static implicit operator CommandResult(Result result)
        {
            if (result == null) return null;

            return new CommandResult()
            {
                Message = result.Message,
                Success = result.Valid
            };
        }
    }

    public interface ICommandDispatcher
    {
        Task<ICommandResult> Dispatch<TParameter>(TParameter command)
            where TParameter : ICommand;
    }
}
