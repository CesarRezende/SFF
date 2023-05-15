using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFF.Domain.BasicInformations.Application.Queriables;
using SFF.Domain.BasicInformations.Application.Queriables.QueryResult;
using SFF.Infra.Repository.Base;

namespace SFF.Infra.Repository.Repositories.BasicInformations
{
    public class BasicInformationsRepository : BaseRepository, IBasicInformationsQueryable
    {

        public BasicInformationsRepository(SFFDbContext dbContext, ILogger<BasicInformationsRepository> logger)
            : base(dbContext, logger)
        {

        }

        public async Task<IEnumerable<FamilyQueryResult>> GetAll()
        {
            try
            {
                var familiesInfo = await _dbContext.Family.
                    Where(x => !x.desativado)
                    .Select(x => new FamilyQueryResult
                    {
                        Id = x.id,
                        Description = x.descricao,
                        Inactivated = x.desativado,

                    }).AsNoTracking().ToListAsync();

                return familiesInfo;

            }
            catch (Exception ex)
            {
                var msgErro = $"An unexpected error occurred while trying to get familys";
                _logger.LogError(ex, msgErro);
                throw;
            }
        }


    }
}
