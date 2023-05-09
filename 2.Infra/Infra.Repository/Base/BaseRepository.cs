using Microsoft.Extensions.Logging;

namespace SFF.Infra.Repository.Base
{
    public abstract class BaseRepository
    {
        protected readonly SFFDbContext _dbContext;

        protected ILogger<BaseRepository> _logger;


        protected BaseRepository(
            SFFDbContext dbContext,
            ILogger<BaseRepository> logger)
        {
            _dbContext = dbContext != null ? dbContext : throw new ArgumentNullException("dbContext");
            _logger = logger != null ? logger : throw new ArgumentNullException("logger");
        }

        public virtual void Dispose()
        {

        }
    }
}
