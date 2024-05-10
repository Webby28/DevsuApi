using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Web.Http.ModelBinding;
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

    public async Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDto cuentaUpdate, int numeroCuenta)
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

    public async Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDto movimientoUpdate, int idMovimiento)
    {
        var dbContext = _appDb.OracleDbContext;

        var movimientoExistente = await dbContext.Movimientos.FirstOrDefaultAsync(c => c.IdMovimiento == idMovimiento);
        var saldoNuevo = 0;
        var saldoActual = movimientoExistente.Saldo;
        if (movimientoExistente.IdMovimiento != 0)
        {
            movimientoExistente.TipoMovimiento = movimientoUpdate.TipoMovimiento;
            movimientoExistente.Valor = movimientoUpdate.Valor;
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

    public async Task<IEnumerable<MovimientosEntity>> ObtenerMovimientoPorFecha(int codCliente, DateTime desde, DateTime hasta)
    {
        var result = await (from movimiento in _appDb.OracleDbContext.Movimientos
                            join cuenta in _appDb.OracleDbContext.Cuenta on movimiento.NumeroCuenta equals cuenta.NumeroCuenta
                            where cuenta.IdCliente == codCliente
                            select movimiento).ToListAsync();

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

    public async Task<bool> TieneMovimiento(int codigoCliente)
    {
        var tieneMovimiento = await (from movimiento in _appDb.OracleDbContext.Movimientos
                                     join cuenta in _appDb.OracleDbContext.Cuenta on movimiento.NumeroCuenta equals cuenta.NumeroCuenta
                                     where cuenta.IdCliente == codigoCliente
                                     select movimiento)
                             .FirstOrDefaultAsync();
        return tieneMovimiento != null;
    }
    /// <summary>
    /// Método que permite la actulizacion del estado de una tabla.
    /// </summary>
    /// <param name="estado">Corresponde al nuevo estado.</param>
    /// <param name="id">Corresponde al id principal de la tabla.</param>
    /// <param name="tabla">Se especifica la tabla donde se actualizará el estado.</param>
    /// <returns>Obtenemos el resultado de operación ejecutada.</returns>
    public async Task<int> ActualizarEstado(char estado, int id, Tabla tabla)
    {
        switch (tabla)
        {
            case Tabla.MOVIMIENTO:
                var movimiento = await ObtenerMovimiento(id);
                if (movimiento == null)
                    return 204; // Otra acción dependiendo de tus requisitos
                movimiento.Estado = estado;
                _appDb.OracleDbContext.Movimientos.Update(movimiento).State = EntityState.Modified;
                break;

            case Tabla.CUENTA:
                var cuenta = await ObtenerCuenta(id);
                if (cuenta == null)
                    return 204; // Otra acción dependiendo de tus requisitos
                cuenta.Estado = estado;
                _appDb.OracleDbContext.Cuenta.Update(cuenta).State = EntityState.Modified;
                break;
            default:
                throw new ReglaNegociosException("Operación seleccionada no corresponde a movimientos.", ErrorType.VALIDACION_PARAMETROS_ENTRADA);
        }

        return await _appDb.OracleDbContext.SaveChangesAsync();
    }   
}