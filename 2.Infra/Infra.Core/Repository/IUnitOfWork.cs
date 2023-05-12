using SFF.Infra.Core.CQRS.Interfaces;
namespace SFF.Infra.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Rollback();
        void Commit();

        Task<CommandResult> RunAsync<TResult>(Func<Task<TResult>> action);
    }
}
