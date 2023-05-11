using Microsoft.EntityFrameworkCore;
using SFF.Infra.Repository.Entities.BasicInformations;

namespace SFF.Infra.Repository.Base
{
    public partial class SFFDbContext : DbContext
    {
        public DbSet<Family> Family { get; set; }
    }

}
