namespace SFF.Infra.Core.CQRS.Interfaces
{

    public interface IQuery
    {
    }

    public interface IQueryResult
    {
    }

    public interface IQueryHandler<TParameter, TResult>
       where TParameter : IQuery
    {
        Task<TResult> Retrieve(TParameter query);
    }

    public interface IQueryDispatcher
    {
        Task<TResult> Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery;
    }
}
