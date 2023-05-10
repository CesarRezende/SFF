using SFF.Domain.Administration.Application.Queriables.QueryResult;

namespace SFF.Domain.Administration.Application.Queriables
{

    public interface IAdministrationQueryable
    {
        Task<IEnumerable<UserQueryResult>> GetAll();
    }
}
