using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SFF.Domain.BasicInformations.Core.Aggregates.FamilyAggregate;
using SFF.Infra.Repository.Entities.Administration;
using SFF.Infra.Repository.EntityConfiguration.Administration;

namespace SFF.Infra.Repository.Base
{
    public partial class SFFDbContext : DbContext
    {
        public DbSet<User> User { get; set; }
    }

}
