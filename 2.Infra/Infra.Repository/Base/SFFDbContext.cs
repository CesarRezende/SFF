using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SFF.Infra.Repository.Entities.Administration;

namespace SFF.Infra.Repository.Base
{
    public class SFFDbContext : DbContext
    {
        public SFFDbContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        {

        }
        
        public DbSet<User> User { get; set; }

        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.Entity is IEntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IEntityBase;

                if (entity == null) continue;

                if (entry.State == EntityState.Added)
                {
                    entity.DataInclusao = DateTime.Now;
                    continue;
                }

                Entry(entity).Property(x => x.DataInclusao).IsModified = false;

                entity.DataAlteracao = DateTime.Now;
            }

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.Entity is IEntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IEntityBase;

                if (entity == null) continue;

                if (entry.State == EntityState.Added)
                {
                    entity.DataInclusao = DateTime.Now;
                    continue;
                }

                Entry(entity).Property(x => x.DataInclusao).IsModified = false;

                entity.DataAlteracao = DateTime.Now;
            }

            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SFFDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            base.OnConfiguring(OptionsBuilder);
        }
    }

}
