using Microsoft.EntityFrameworkCore;
using SFF.Infra.Repository.EntityConfiguration.Administration;

namespace SFF.Infra.Repository.Base
{
    public partial class SFFDbContext : DbContext
    {
        public SFFDbContext(DbContextOptions optionsBuilder) : base(optionsBuilder)
        {
        }
        
        public override int SaveChanges()
        {
            var modifiedEntries = ChangeTracker.Entries().Where(x => x.Entity is IEntityBase && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entry in modifiedEntries)
            {
                var entity = entry.Entity as IEntityBase;

                if (entity == null) continue;

                if (entry.State == EntityState.Added)
                {
                    entity.createdTime = DateTime.Now;
                    continue;
                }

                Entry(entity).Property(x => x.createdTime).IsModified = false;

                entity.updatedTime = DateTime.Now;
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
                    entity.createdTime = DateTime.Now;
                    continue;
                }

                Entry(entity).Property(x => x.createdTime).IsModified = false;

                entity.updatedTime = DateTime.Now;
            }

            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SFFDbContext).Assembly);
            modelBuilder.RegisterAdministrationDbConfiguration();
            modelBuilder.RegisterBasicInformationsDbConfiguration();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder OptionsBuilder)
        {
            base.OnConfiguring(OptionsBuilder);
        }

    }

}
