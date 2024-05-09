using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Infrastructure.Database;

/// <summary>
/// Db Context para inserts, updates, deletes
/// </summary>
public sealed class OracleDbContext : DbContext, IApplicationDbContext
{
    public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
    {
    }

    public DbSet<ClienteEntity> Cliente { get; set; }
    public DbSet<CuentaEntity> Cuenta { get; set; }
    public DbSet<MovimientosEntity> Movimientos { get; set; }
    public DbSet<PersonaEntity> Persona { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}