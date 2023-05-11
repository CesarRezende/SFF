using SFF.Infra.Core.CQRS.Interfaces;

namespace SFF.Domain.BasicInformations.Application.Queriables.QueryResult
{
    public class FamilyQueryResult : IQueryResult
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public bool Inactivated { get; set; }
    }
}
