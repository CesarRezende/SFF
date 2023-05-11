using SFF.Domain.BasicInformations.Application.Queriables.QueryResult;

namespace SFF.Domain.BasicInformations.Application.Queriables
{

    public interface IBasicInformationsQueryable
    {
        Task<IEnumerable<FamilyQueryResult>> GetAll();
    }
}
