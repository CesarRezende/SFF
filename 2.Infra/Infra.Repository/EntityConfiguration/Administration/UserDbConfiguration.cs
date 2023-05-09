using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFF.Infra.Repository.Entities.Administration;

namespace SFF.Infra.Repository.EntityConfiguration.Administration
{
    public class UserDbConfiguration : IEntityTypeConfiguration<User>
    {
        public virtual void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User", "administration");
            builder.HasKey(t => t.Id);
            builder.Property(x => x.CreatedTime).IsRequired();
            builder.Property(x => x.UpdatedTime).IsRequired();
            builder.Property(x => x.Active).IsRequired();
        }
    }
}
