using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFF.Domain.Administration.Core.Repositories;
using SFF.Infra.Repository.Base;
using SFF.Infra.Repository.MapExtension.BasicInformations;

using domain = SFF.Domain.BasicInformations.Core.Aggregates.FamilyAggregate;
using db = SFF.Infra.Repository.Entities.BasicInformations;
using SFF.Infra.Core.Helper;

namespace SFF.Infra.Repository.Repositories.BasicInformations
{

    public class FamilyRepository : BaseEntityRepository<db.Family>, IFamilyRepository
    {

        public FamilyRepository(SFFDbContext dbContext, ILogger<FamilyRepository> logger)
            : base(dbContext, logger)
        {

        }

        private static readonly Func<SFFDbContext, long, Task<db.Family>> _findFamily =
            EF.CompileAsyncQuery((SFFDbContext contexto, long familyId) =>
                contexto.Family.AsNoTracking().FirstOrDefault(x => x.id == familyId));

        private static readonly Func<SFFDbContext, string, Task<bool>> _existFamily =
            EF.CompileAsyncQuery((SFFDbContext contexto, string familyDescr) =>
                contexto.Family.AsNoTracking().Any(x => x.descricao == familyDescr));


        public Task DeleteAsync(domain.Family entity)
        {

            try
            {
                var family = entity.ToDbEntity();
                if (_dbContext.Family.Entry(family).State == EntityState.Detached)
                    _dbContext.Family.Attach(family);

                _dbContext.Family.Remove(family);

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to delete family {entity.Id} from the database");
                throw;
            }
        }

        public override void Dispose()
        {

        }


        public async Task<domain.Family> GetByIdAsync(long id)
        {

            try
            {
                var family = await _findFamily(_dbContext, id);

                if (family is null) return null;

                return family.ToDomain();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to get the family {id} from the database");
                throw;
            }

        }

        public async Task<bool> ExiteFamiliaAsync(string description)
        {

            try
            {
                return await _existFamily(_dbContext, description);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to check the family {description} from the database");
                throw;
            }

        }

        public Task InsertAsync(domain.Family family)
        {
            var newFamily = family.ToDbEntity();

            try
            {
                _dbContext.Family.Add(newFamily);
                return _dbContext.SaveChangesAsync();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to insert a family {family.Id} in the database");
                throw;
            }
            finally
            {
                _logger.LogDebug($"Family: {newFamily.ToJsonFormat()}");
            }
        }

        public async Task UpdateAsync(domain.Family familyUpdated)
        {

            var updateFamily = familyUpdated.ToDbEntity();

            try
            {
                var currentFamily = await _dbContext.Family.FirstAsync(x => x.id == updateFamily.id);
                currentFamily.descricao = updateFamily.descricao;
                currentFamily.desativado = updateFamily.desativado;
                currentFamily.hora_atualizacao = DateTime.Now;

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unexpected error occurred while tring to update the family {updateFamily.id} in the database");
                throw;
            }
            finally
            {
                _logger.LogDebug($"Family: {updateFamily.ToJsonFormat()}");
            }
        }
    }
}
