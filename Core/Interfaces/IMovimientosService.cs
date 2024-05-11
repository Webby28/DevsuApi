using Core.Contracts.Models;
using Microsoft.AspNetCore.JsonPatch;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosService
{
    Task<CuentaEntity> InsertarCuenta(CuentaRequest cuenta);

    Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest movimiento);

    Task<CuentaEntity> ObtenerCuenta(int codigoCuenta);

    Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento);

    Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDto cuentaUpdate, int numeroCuenta);

    Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDto movimientoUpdate, int codigoMovimiento);

    Task<bool> EliminarCuenta(int numeroCuenta);

    Task<bool> EliminarMovimiento(int codigoMovimiento);

    Task<List<MovimientoReporte>> GenerarReporte(string rangoFechas, int codigoCliente);

    Task<byte[]> GenerarReportePDF(string rangoFechas, int codigoCliente);

    Task<int> ActualizarEstado(string estado, int id, Tabla tabla);
}