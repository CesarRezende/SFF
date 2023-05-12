using SFF.Domain.SharedKernel.Base;
using domain = SFF.Domain.BasicInformations.Core.Aggregates.FamilyAggregate;

namespace SFF.Domain.Administration.Core.Repositories
{
    public  interface IFamilyRepository: IRepository<domain.Family>
    {
        Task<bool> ExiteFamiliaAsync(string description);
    }
}
