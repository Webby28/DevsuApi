using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Infrastructure.Database.Configuration;

public class MovimientosConfiguration : IEntityTypeConfiguration<MovimientosEntity>
{
    public void Configure(EntityTypeBuilder<MovimientosEntity> builder)
    {
        builder.ToTable("MOVIMIENTOS", "API_DEVSU");
        builder.HasKey(cc => cc.IdMovimiento);
        builder.Property(cc => cc.IdMovimiento).HasColumnName("ID_MOVIMIENTO").IsUnicode(false);
        builder.Property(cc => cc.Fecha).HasColumnName("FECHA").IsUnicode(false);
        builder.Property(cc => cc.TipoMovimiento).HasColumnName("TIPO_MOVIMIENTO").IsUnicode(false);
        builder.Property(cc => cc.Valor).HasColumnName("VALOR").IsUnicode(false);
        builder.Property(cc => cc.Saldo).HasColumnName("SALDO").IsUnicode(false);
    }
}