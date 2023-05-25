using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFF.Infra.Repository.Entities.Administration;

namespace SFF.Infra.Repository.EntityConfiguration.Administration
{
    public class UserDbConfiguration : IEntityTypeConfiguration<User>
    {
        public virtual void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("usuario", "public");
            builder.HasKey(t => t.id).HasName("id");
            builder.Property(x => x.nome).HasColumnName("nome").IsRequired();
            builder.Property(x => x.login).HasColumnName("login").IsRequired();
            builder.Property(x => x.administrator).HasColumnName("administrator").IsRequired();
            builder.Property(x => x.desativado).HasColumnName("desativado").IsRequired();
            builder.Property(x => x.hora_criacao).HasColumnName("hora_criacao").IsRequired();
            builder.Property(x => x.hora_atualizacao).HasColumnName("hora_atualizacao");


            builder.HasOne(x => x.sessao).WithOne(x => x.User).HasForeignKey<Session>(x => x.usuario_id);
        }
    }
}
