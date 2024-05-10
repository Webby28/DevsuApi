using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosService
{
    Task<CuentaEntity> InsertarCuenta(CuentaRequest cuenta);

    Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest movimiento);

    Task<CuentaEntity> ObtenerCuenta(int codigoCuenta);

    Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento);

    Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDTO cuentaUpdate, int numeroCuenta);

    Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDto movimientoUpdate, int codigoMovimiento);

    Task<bool> EliminarCuenta(int numeroCuenta);

    Task<bool> EliminarMovimiento(int codigoMovimiento);

    Task<byte[]> GenerarReporte(string rangoFechas, int codigoCliente);
}