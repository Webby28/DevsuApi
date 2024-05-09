using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Infrastructure.Database;

public interface IApplicationDbContext
{
    public DbSet<ClienteEntity> Cliente { get; set; }
    public DbSet<CuentaEntity> Cuenta { get; set; }
    public DbSet<MovimientosEntity> Movimientos { get; set; }
    public DbSet<PersonaEntity> Persona { get; set; }

    int SaveChanges();

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());

    ChangeTracker ChangeTracker { get; }
}