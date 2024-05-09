using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosRepository
{
    Task<CuentaEntity> InsertarCuenta(CuentaEntity cuenta);

    Task<CuentaEntity> ObtenerCuenta(int numeroCuenta);

    Task<MovimientosEntity> InsertarMovimiento(MovimientosEntity movimientos);

    Task<MovimientosEntity> ObtenerMovimiento(int idMovimiento);

    Task<CuentaEntity> ActualizarCuenta(CuentaRequest cuentaUpdate, int numeroCuenta);

    Task<MovimientosEntity> ActualizarMovimiento(MovimientosRequest movimientoUpdate, int idMovimiento);

    Task<bool> ExisteCuenta(int numeroCuenta);

    Task<bool> ExisteMovimiento(int idMovimiento);

    Task<bool> TieneCuenta(int codigoPersona, string tipoCuenta);

    Task<int> SaldoActual(int numeroCuenta);

    Task<bool> EliminarCuenta(int codigoPersona);

    Task<bool> EliminarMovimiento(int codigoPersona);
}