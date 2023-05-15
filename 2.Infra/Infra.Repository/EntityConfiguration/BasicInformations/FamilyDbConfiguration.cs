using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFF.Infra.Repository.Entities.BasicInformations;

namespace SFF.Infra.Repository.EntityConfiguration.Administration
{
    public class FamilyDbConfiguration : IEntityTypeConfiguration<Family>
    {
        public virtual void Configure(EntityTypeBuilder<Family> builder)
        {
            builder.ToTable("familia", "public");
            builder.HasKey(t => t.id).HasName("id");
            builder.Property(x => x.descricao).HasColumnName("descricao").IsRequired();
            builder.Property(x => x.desativado).HasColumnName("desativado").IsRequired();
            builder.Property(x => x.hora_criacao).HasColumnName("hora_criacao").IsRequired();
            builder.Property(x => x.hora_atualizacao).HasColumnName("hora_atualizacao");
        }
    }
}
