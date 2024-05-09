using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Infrastructure.Database.Configuration;

public class PersonaConfiguration : IEntityTypeConfiguration<PersonaEntity>
{
    public void Configure(EntityTypeBuilder<PersonaEntity> builder)
    {
        builder.ToTable("PERSONA", "API_DEVSU");
        builder.HasKey(cc => cc.IdPersona);
        builder.Property(cc => cc.IdPersona).HasColumnName("ID_PERSONA").IsUnicode(false);
        builder.Property(cc => cc.Nombre).HasColumnName("NOMBRE").IsUnicode(false);
        builder.Property(cc => cc.Genero).HasColumnName("GENERO").IsUnicode(false);
        builder.Property(cc => cc.Edad).HasColumnName("EDAD").IsUnicode(false);
        builder.Property(cc => cc.Identificacion).HasColumnName("IDENTIFICACION").IsUnicode(false);
        builder.Property(cc => cc.Direccion).HasColumnName("DIRECCION").IsUnicode(false);
        builder.Property(cc => cc.Telefono).HasColumnName("TELEFONO").IsUnicode(false);
    }
}