using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Infrastructure.Database.Configuration;

public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
{
    public void Configure(EntityTypeBuilder<ClienteEntity> builder)
    {
        builder.ToTable("CLIENTE", "API_DEVSU");
        builder.HasKey(cc => cc.IdCliente);
        builder.Property(cc => cc.IdCliente).HasColumnName("ID_CLIENTE").IsUnicode(false);
        builder.Property(cc => cc.PersonaId).HasColumnName("PERSONA_ID").IsUnicode(false);
        builder.Property(cc => cc.Contraseña).HasColumnName("CONTRASEÑA").IsUnicode(false);
        builder.Property(cc => cc.Estado).HasColumnName("ESTADO").IsUnicode(false);
    }
}