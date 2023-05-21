using SFF.Domain.SharedKernel.Base;
using domain = SFF.Domain.Administration.Core.Aggregates.UserAggregate;

namespace SFF.Domain.Administration.Core.Repositories
{
    public  interface IUserRepository: IRepository<domain.User>
    {
        Task<domain.User> GetByLoginAsync(string login);
    }
}
