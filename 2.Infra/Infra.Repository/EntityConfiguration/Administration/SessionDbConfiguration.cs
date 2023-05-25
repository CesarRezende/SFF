using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFF.Infra.Repository.Entities.Administration;

namespace SFF.Infra.Repository.EntityConfiguration.Administration
{
    public class SessionDbConfiguration : IEntityTypeConfiguration<Session>
    {
        public virtual void Configure(EntityTypeBuilder<Session> builder)
        {
            builder.ToTable("sessao", "public");
            builder.HasKey(t => t.id).HasName("id");
            builder.Property(x => x.ip).HasColumnName("ip").IsRequired();
            builder.Property(x => x.data_expiracao).HasColumnName("data_expiracao").IsRequired();
            builder.Property(x => x.refresh_token).HasColumnName("refresh_token").IsRequired();
            builder.Property(x => x.data_expiracao_refresh_token).HasColumnName("data_expiracao_refresh_token").IsRequired();
            builder.Property(x => x.hora_criacao).HasColumnName("hora_criacao").IsRequired();
            builder.Property(x => x.hora_atualizacao).HasColumnName("hora_atualizacao");
            builder.Property(x => x.usuario_id).HasColumnName("usuario_id").IsRequired();

        }
    }
}
