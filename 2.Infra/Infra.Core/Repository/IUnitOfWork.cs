using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFF.Infra.Core.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        void BeginTransaction();
        void Rollback();
        void Commit();
    }
}
