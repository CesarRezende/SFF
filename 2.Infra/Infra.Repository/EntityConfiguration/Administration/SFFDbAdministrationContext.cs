using Microsoft.EntityFrameworkCore;
using SFF.Infra.Repository.Entities.Administration;

namespace SFF.Infra.Repository.Base
{
    public partial class SFFDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<Session> Session { get; set; }
    }

}
