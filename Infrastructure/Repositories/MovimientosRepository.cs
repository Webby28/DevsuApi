using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Interfaces;
using WebApi.Infrastructure.Database;

namespace WebApi.Infrastructure.Repositories;

public class MovimientosRepository : IMovimientosRepository
{
    private readonly IAppDb _appDb;
    private readonly IMapper _mapper;

    public MovimientosRepository(IAppDb appDb, IMapper mapper)
    {
        _appDb = appDb;
        _mapper = mapper;
    }

    public async Task<CuentaEntity> InsertarCuenta(CuentaEntity cuenta)
    {
        var dbContext = _appDb.OracleDbContext;
        dbContext.Cuenta.Add(cuenta);
        await dbContext.SaveChangesAsync();
        return cuenta;
    }

    public async Task<MovimientosEntity> InsertarMovimiento(MovimientosEntity movimientos)
    {
        var dbContext = _appDb.OracleDbContext;
        dbContext.Movimientos.Add(movimientos);
        await dbContext.SaveChangesAsync();
        return movimientos;
    }

    public async Task<CuentaEntity> ActualizarCuenta(CuentaRequest cuentaUpdate, int numeroCuenta)
    {
        throw new NotImplementedException();
    }

    public async Task<MovimientosEntity> ActualizarMovimiento(MovimientosRequest movimientoUpdate, int idMovimiento)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EliminarCuenta(int numeroCuenta)
    {
        var dbContext = _appDb.OracleDbContext;
        var cuenta = await dbContext.Cuenta.FindAsync(numeroCuenta);
        if (cuenta == null)
        {
            return false;
        }
        dbContext.Cuenta.Remove(cuenta);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarMovimiento(int idMovimiento)
    {
        var dbContext = _appDb.OracleDbContext;
        var movimiento = await dbContext.Movimientos.FindAsync(idMovimiento);
        if (movimiento == null)
        {
            return false;
        }
        dbContext.Movimientos.Remove(movimiento);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExisteCuenta(int numeroCuenta)
    {
        var existeCuenta = await _appDb.OracleDbContext.Cuenta
            .Where(p => p.NumeroCuenta == numeroCuenta)
            .FirstOrDefaultAsync();

        return existeCuenta != null;
    }

    public async Task<bool> ExisteMovimiento(int idMovimiento)
    {
        var existeMovimiento = await _appDb.OracleDbContext.Movimientos
            .Where(p => p.IdMovimiento == idMovimiento)
            .FirstOrDefaultAsync();

        return existeMovimiento != null;
    }

    public async Task<CuentaEntity> ObtenerCuenta(int numeroCuenta)
    {
        var result = await _appDb.OracleDbContext
       .Cuenta
       .Where(p => p.NumeroCuenta == numeroCuenta)
       .FirstOrDefaultAsync();

        return result;
    }

    public async Task<MovimientosEntity> ObtenerMovimiento(int idMovimiento)
    {
        var result = await _appDb.OracleDbContext
      .Movimientos
      .Where(p => p.IdMovimiento == idMovimiento)
      .FirstOrDefaultAsync();

        return result;
    }

    public async Task<bool> TieneCuenta(int codigoCliente, string tipoCuenta)
    {
        var tieneCuenta = await _appDb.OracleDbContext.Cuenta
            .AnyAsync(p => p.IdCliente == codigoCliente && p.TipoCuenta == tipoCuenta);

        return tieneCuenta;
    }

    public async Task<int> SaldoActual(int numeroCuenta)
    {
        var ultimoSaldo = await _appDb.OracleDbContext.Movimientos
         .Where(m => m.NumeroCuenta == numeroCuenta)
         .OrderByDescending(m => m.FechaRegistro)
    .Select(m => new { m.Saldo, m.NumeroCuenta })
         .FirstOrDefaultAsync();

        if (ultimoSaldo != null)
        {
            return ultimoSaldo.Saldo;
        }
        else
        {
            var primerMovimiento = await _appDb.OracleDbContext.Cuenta
            .Where(p => p.NumeroCuenta == numeroCuenta)
            .FirstOrDefaultAsync();
            if (primerMovimiento != null)
            {
                return primerMovimiento.SaldoInicial;
            }
            else
            {
                return 0;
            }
        }
    }

    public async Task ActualizarSaldoCuenta(int numeroCuenta, int saldoRestante)
    {
        var cuenta = await _appDb.OracleDbContext.Cuenta
        .FirstOrDefaultAsync(p => p.NumeroCuenta == numeroCuenta);

        if (cuenta != null)
        {
            cuenta.SaldoInicial = saldoRestante;
            await _appDb.OracleDbContext.SaveChangesAsync();
        }
    }
}