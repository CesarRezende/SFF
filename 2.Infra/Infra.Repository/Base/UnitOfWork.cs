
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SFF.Infra.Core.Repository;

namespace SFF.Infra.Repository.Base
{


    public class UnitOfWork : IUnitOfWork
    {
        private readonly SFFDbContext Context;

        private IDbContextTransaction _transaction;

        public UnitOfWork(SFFDbContext context)
        {
            Context = context;
        }

        public void BeginTransaction()
        {
            _transaction = Context.Database.BeginTransaction(IsolationLevel.ReadUncommitted);
        }

        public void Commit()
        {
            _transaction?.Commit();
        }

        public void Rollback()
        {
            _transaction?.Rollback();
        }

        public void Dispose()
        {
            _transaction?.Dispose();

        }
    }
}
