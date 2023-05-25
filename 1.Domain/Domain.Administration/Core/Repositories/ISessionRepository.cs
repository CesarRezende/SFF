using SFF.Domain.SharedKernel.Base;
using domain = SFF.Domain.Administration.Core.Aggregates.SessionAggegate;

namespace SFF.Domain.Administration.Core.Repositories
{
    public  interface ISessionRepository: IRepository<domain.Session>
    {
        Task<domain.Session> GetByRefreshTokenAsync(string refreshToken);
    }
}
