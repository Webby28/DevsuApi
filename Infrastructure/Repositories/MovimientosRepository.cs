using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
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

    public async Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDTO cuentaUpdate, int numeroCuenta)
    {
        var dbContext = _appDb.OracleDbContext;

        var cuentaExistente = await dbContext.Cuenta.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);

        if (cuentaExistente != null)
        {
            cuentaExistente.TipoCuenta = cuentaUpdate.TipoCuenta;
            cuentaExistente.Estado = cuentaUpdate.Estado;

            await dbContext.SaveChangesAsync();

            return cuentaExistente;
        }
        else
        {
            throw new ReglaNegociosException("La cuenta no existe.", ErrorType.CUENTA_NO_EXISTE);
        }
    }

    public async Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDTO movimientoUpdate, int idMovimiento)
    {
        var dbContext = _appDb.OracleDbContext;

        var movimientoExistente = await dbContext.Movimientos.FirstOrDefaultAsync(c => c.IdMovimiento == idMovimiento);
        var saldoNuevo = 0;
        var saldoActual = movimientoExistente.Saldo;
        if (movimientoExistente != null)
        {
            movimientoExistente.TipoMovimiento = movimientoUpdate.TipoMovimiento;
            movimientoExistente.Valor= movimientoUpdate.Valor;
            switch (movimientoUpdate.TipoMovimiento)
            {
                case '0':
                    saldoNuevo = saldoActual + movimientoUpdate.Valor;
                    break;

                case '1':
                    saldoNuevo = saldoActual - movimientoUpdate.Valor;
                    break;
            }
            movimientoExistente.Saldo = saldoNuevo;
            await dbContext.SaveChangesAsync();

            return movimientoExistente;
        }
        else
        {
            throw new ReglaNegociosException("La cuenta no existe.", ErrorType.CUENTA_NO_EXISTE);
        }
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

    public async Task<int> ActualizarSaldo(int idMovimiento)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> CuentaConMovimiento(int numeroCuenta)
    {
        var conMovimiento = await _appDb.OracleDbContext.Movimientos
           .Where(p => p.NumeroCuenta == numeroCuenta)
           .FirstOrDefaultAsync();

        return conMovimiento != null;
    }
}