using Flunt.Notifications;
using SFF.Domain.SharedKernel;

namespace SFF.SharedKernel
{
    public class OperationResult : OperationResultBase<OperationResult>
    {
        public OperationResult()
        {

        }

        public OperationResult(IReadOnlyCollection<Notification> erros)
        {
            AddNotifications(erros);
        }

    }


    public class OperationResult<T> : OperationResultBase<OperationResult<T>>
    {
        public T Data { get; set; }

        public OperationResult()
        {

        }

        public OperationResult(T data)
        {
            Data = data;
        }

        public OperationResult(T data, IReadOnlyCollection<Notification> erros)
        {
            Data = data;
            AddNotifications(erros);
        }


    }

}

public abstract class OperationResultBase<OP> : ValueObject
    where OP : OperationResultBase<OP>
{
    private bool _notFound { get; set; }
    public IEnumerable<string> Erros { get => Notifications.Select(x => x.Message); }

    public bool Succeded => IsValid && !IsNotFound;

    public bool IsNotFound => _notFound;

    public OP AddError(string erroMsg) 
    {
        AddNotification("", erroMsg);
        return this as OP;
    }

    public OP AddError(Notification erroMsg)
    {
        AddNotification(erroMsg);
        return this as OP;
    }


    public OP AddErrors(IEnumerable<string> erroMsg)
    {
        IList<Notification> notifications = erroMsg.Select(x => new Notification("", x)).ToList();
        AddNotifications(notifications);
        return this as OP;
    }

    public OP AddErrors(IList<Notification> erroMsgs)
    {
        AddNotifications(erroMsgs);
        return this as OP;
    }

    public OP SetAsNotFound()
    {
        _notFound = true;
        return this as OP;
    }
}