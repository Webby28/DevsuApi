using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosService
{
    Task<CuentaEntity> InsertarCuenta(CuentaRequest cuenta);

    Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest cliente);

    Task<CuentaEntity> ObtenerCuenta(int codigoCuenta);

    Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento);

    Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDTO cuentaUpdate, int codigoPersona);

    Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDTO movimientoUpdate, int codigoMovimiento);

    Task<bool> EliminarCuenta(int codigoPersona);

    Task<bool> EliminarMovimiento(int codigoCliente);

    Task<byte[]> GenerarReporte(string rangoFechas, int codigoCliente);
}