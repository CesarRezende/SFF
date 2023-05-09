using Flunt.Notifications;
using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Infra.Core.CQRS.Models
{

    public class Result :  Notifiable<Notification>

    {
        public Result() { }

        public Result(bool valid, string message)
        {
            Valid = valid;
            Message = message;
        }

        public bool Invalid { get { return !Valid; } }

        public string Message { get; set; }

        public bool Valid { get; set; }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static implicit operator Result(CommandResult result)
        {
            return result.Success ? Success() : Fail(result.Message);
        }

        public static Result Success()
        {
            return new Result(true, string.Empty);
        }
    }

    public class Result<T> : Result
    {
        public Result() { }

        private Result(bool valid, string message, T data = default) : base(valid, message)
        {
            Data = data;
        }

        public T Data { get; set; }

        public static new Result<T> Fail(string message)
        {
            return new Result<T>(false, message);
        }

        public static Result<T> Fail(IEnumerable<string> messages)
        {
            return new Result<T>(false, string.Join(", ", messages));
        }

        public static Result<T> Fail(string message, T data) => new Result<T>(false, message, data);

        public static Result<T> Success(T data)
        {
            return new Result<T>(true, string.Empty, data);
        }
    }
}
