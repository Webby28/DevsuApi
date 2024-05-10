using WebApi.Core.Contracts.Entities;

namespace WebApi.Core.Interfaces;

public interface IMovimientosRepository
{
    Task<CuentaEntity> InsertarCuenta(CuentaEntity cuenta);

    Task<CuentaEntity> ObtenerCuenta(int numeroCuenta);

    Task<MovimientosEntity> InsertarMovimiento(MovimientosEntity movimientos);

    Task<MovimientosEntity> ObtenerMovimiento(int idMovimiento);

    Task<IEnumerable<MovimientosEntity>> ObtenerMovimientoPorFecha(int codCliente, DateTime desde, DateTime hasta);

    Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDTO cuentaUpdate, int numeroCuenta);

    Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDto movimientoUpdate, int idMovimiento);

    Task<bool> ExisteCuenta(int numeroCuenta);

    Task<bool> ExisteMovimiento(int idMovimiento);

    Task<bool> TieneCuenta(int codigoCliente, string tipoCuenta);

    Task<int> SaldoActual(int numeroCuenta);

    Task<bool> EliminarCuenta(int numeroCuenta);

    Task<bool> EliminarMovimiento(int idMovimiento);

    Task<int> ActualizarSaldo(int idMovimiento);

    Task<bool> CuentaConMovimiento(int numeroCuenta);

    Task<bool> TieneMovimiento(int codigoCliente);
}