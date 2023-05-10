using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Infra.Repository.Base;
using SFF.Infra.Repository.MapExtension.Administration;
using SFF.SharedKernel.Helpers;

using domain = SFF.Domain.Administration.Core.Aggregates.UserAggregate;
using db = SFF.Infra.Repository.Entities.Administration;

namespace SFF.Infra.Repository.Repositories.Administration
{

    public class UserRepository : BaseRepository, IUserRepository
    {

        public UserRepository(SFFDbContext dbContext, ILogger<UserRepository> logger)
            : base(dbContext, logger)
        {

        }

        private static readonly Func<SFFDbContext, long, Task<db.User>> _findUser =
            EF.CompileAsyncQuery((SFFDbContext contexto, long userId) =>
                contexto.User.AsNoTracking().FirstOrDefault(x => x.id == userId));


        public Task DeleteAsync(domain.User entity)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {

        }


        public async Task<domain.User> GetByIdAsync(long id)
        {

            try
            {
                var user = await _findUser(_dbContext, id);

                if (user is null) return null;

                return user.ToDomain();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to get the user {id} from the database");
                throw;
            }

        }

        public Task InsertAsync(domain.User user)
        {
            var newUser = user.ToDbEntity();

            try
            {
                _dbContext.User.Add(newUser);
                return _dbContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to insert a user {user.Id} in the database");
                throw;
            }
            finally
            {
                _logger.LogDebug($"User: {newUser.ToJsonFormat()}");
            }
        }

        public async Task UpdateAsync(domain.User userUpdated)
        {

            var updateUser = userUpdated.ToDbEntity();
            //updateUser.UpdatedTime = DateTimeOffset.UtcNow;

            try
            {
                var currentUser = await _dbContext.User.FirstAsync(x => x.id == updateUser.id);

                //currentUser.Active = updateUser.Active;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to update the user {updateUser.id} in the database");
                throw;
            }
            finally
            {
                _logger.LogDebug($"User: {updateUser.ToJsonFormat()}");
            }
        }
    }
}
