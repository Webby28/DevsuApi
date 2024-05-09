using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosService
{
    Task<CuentaEntity> InsertarCuenta(CuentaRequest cuenta);
    Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest cliente);
    Task<CuentaEntity> ObtenerCuenta(int codigoCuenta);
    Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento);
    Task<CuentaEntity> ActualizarCuenta(CuentaRequest cuentaUpdate, CodigoPersonaRequest codigoPersona);
    Task<MovimientosEntity> ActualizarMovimiento(int PersonaId, ClienteUpdateRequest clienteUpdate, string passwordAnterior);
    Task<bool> EliminarCuenta(int codigoPersona);
    Task<bool> EliminarMovimiento(int codigoCliente);
}