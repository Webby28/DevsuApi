using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Infrastructure.Database.Configuration;

public class CuentaConfiguration : IEntityTypeConfiguration<CuentaEntity>
{
    public void Configure(EntityTypeBuilder<CuentaEntity> builder)
    {
        builder.ToTable("CUENTA", "API_DEVSU");
        builder.HasKey(cc => cc.NumeroCuenta);
        builder.Property(cc => cc.NumeroCuenta).HasColumnName("NUMERO_CUENTA").IsUnicode(false);
        builder.Property(cc => cc.TipoCuenta).HasColumnName("TIPO_CUENTA").IsUnicode(false);
        builder.Property(cc => cc.SaldoInicial).HasColumnName("SALDO_INICIAL").IsUnicode(false);
        builder.Property(cc => cc.Estado).HasColumnName("ESTADO").IsUnicode(false);
    }
}