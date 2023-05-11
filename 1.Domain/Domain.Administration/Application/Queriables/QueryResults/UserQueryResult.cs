using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.Administration.Application.Queriables.QueryResult
{
    public class UserQueryResult: IQueryResult
    {
        public long Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public bool Administrator { get; set; }
        public bool Inactivated { get; set; }
    }
}
