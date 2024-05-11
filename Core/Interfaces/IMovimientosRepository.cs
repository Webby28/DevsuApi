using Core.Contracts.Models;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosRepository
{
    Task<CuentaEntity> InsertarCuenta(CuentaEntity cuenta);

    Task<CuentaEntity> ObtenerCuenta(int numeroCuenta);

    Task<MovimientosEntity> InsertarMovimiento(MovimientosEntity movimientos);

    Task<MovimientosEntity> ObtenerMovimiento(int idMovimiento);

    Task<IEnumerable<MovimientosEntity>> ObtenerMovimientoPorFecha(int codCliente, DateTime desde, DateTime hasta);

    Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDto cuentaUpdate, int numeroCuenta);

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
    Task<int> ActualizarEstado(string estado, int id, Tabla tabla);
    Task<bool> TipoCuentaDuplicada(int numeroCuenta, string tipoCuenta);
    Task<ListaCuentas> ObtenerCuentas(int codigoCliente);
}