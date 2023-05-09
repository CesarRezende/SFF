using SFF.Infra.Core.CQRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFF.Infra.Core.Repository
{
    public interface IUnitOfWorkWithTransactionScope : IDisposable
    {
        void BeginTransaction();

        void BeginNewTransaction();

        void Commit();

        void Rollback();

        Task<Result<TResult>> RunAsync<TResult>(Func<Task<TResult>> action);
    }
}
