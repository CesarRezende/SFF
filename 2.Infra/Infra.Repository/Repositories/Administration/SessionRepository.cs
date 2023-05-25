using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Infra.Repository.Base;
using SFF.Infra.Repository.MapExtension.Administration;

using domain = SFF.Domain.Administration.Core.Aggregates.SessionAggegate;
using db = SFF.Infra.Repository.Entities.Administration;
using SFF.Infra.Core.Helper;

namespace SFF.Infra.Repository.Repositories.Administration
{

    public class SessionRepository : BaseRepository, ISessionRepository
    {

        public SessionRepository(SFFDbContext dbContext, ILogger<SessionRepository> logger)
            : base(dbContext, logger)
        {

        }

        private static readonly Func<SFFDbContext, long, Task<db.Session>> _findSession =
            EF.CompileAsyncQuery((SFFDbContext contexto, long sessionId) =>
                contexto.Session.Include(x => x.User).AsNoTracking().FirstOrDefault(x => x.id == sessionId));

        private static readonly Func<SFFDbContext, string, Task<db.Session>> _findRefreshTokenAsync =
            EF.CompileAsyncQuery((SFFDbContext contexto, string refreshToken) =>
                contexto.Session.Include(x => x.User).AsNoTracking().FirstOrDefault(x => x.refresh_token == refreshToken));



        public Task DeleteAsync(domain.Session entity)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {

        }


        public async Task<domain.Session> GetByIdAsync(long id)
        {

            try
            {
                var session = await _findSession(_dbContext, id);

                if (session is null) return null;

                return session.ToDomain();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to get the session {id} from the database");
                throw;
            }

        }

        public async Task<domain.Session> GetByRefreshTokenAsync(string refreshToken)
        {

            try
            {
                var session = await _findRefreshTokenAsync(_dbContext, refreshToken);

                if (session is null) return null;

                return session.ToDomain();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to get the session by refresh token from the database");
                throw;
            }

        }

        public Task InsertAsync(domain.Session session)
        {
            var newSession = session.ToDbEntity();

            try
            {
                _dbContext.Session.Add(newSession);
                return _dbContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to insert a session {session.Id} in the database");
                throw;
            }
            finally
            {
                _logger.LogDebug($"Session: {newSession.ToJsonFormat()}");
            }
        }

        public async Task UpdateAsync(domain.Session sessionUpdated)
        {

            var updateSession = sessionUpdated.ToDbEntity();

            try
            {
                var currentSession = await _dbContext.Session.FirstAsync(x => x.id == updateSession.id);

                currentSession.ip = updateSession.ip;
                currentSession.data_expiracao = updateSession.data_expiracao;
                currentSession.refresh_token = updateSession.refresh_token;
                currentSession.data_expiracao_refresh_token = updateSession.data_expiracao_refresh_token;
                currentSession.hora_criacao = updateSession.hora_criacao;
                currentSession.hora_atualizacao = updateSession.hora_atualizacao;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to update the session {updateSession.id} in the database");
                throw;
            }
            finally
            {
                _logger.LogDebug($"Session: {updateSession.ToJsonFormat()}");
            }
        }
    }
}
