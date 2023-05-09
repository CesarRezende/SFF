using SFF.Infra.Core.CQRS.Models;
using System.Transactions;

namespace SFF.Infra.Core.Repository
{

    public class UnitOfWorkWithTransactionScope : IUnitOfWorkWithTransactionScope
    {

        private TransactionScope _transactionScope;

        public UnitOfWorkWithTransactionScope()
        {
        }

        public void BeginTransaction()
        {
            _transactionScope = new TransactionScope
            (
                TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadUncommitted,
                    Timeout = TimeSpan.FromMinutes(10)
                },
                TransactionScopeAsyncFlowOption.Enabled
            );
        }

        public void BeginNewTransaction()
        {
            Dispose();
            BeginTransaction();
        }

        public void Commit()
        {
            _transactionScope?.Complete();
        }

        public void Dispose()
        {
            _transactionScope?.Dispose();
        }

        public void Rollback()
        {
            Dispose();
        }

        public async Task<Result<TResult>> RunAsync<TResult>(Func<Task<TResult>> action)
        {
            try
            {
                var result = await action.Invoke();

                Commit();

                return Result<TResult>.Success(result);
            }
            catch (Exception ex)
            {
                Rollback();

                return Result<TResult>.Fail(ex.Message);
            }
        }
    }
}
