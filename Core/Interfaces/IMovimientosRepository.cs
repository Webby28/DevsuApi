using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosRepository
{
    Task<CuentaEntity> InsertarCuenta(CuentaEntity cuenta);
    Task<CuentaEntity> ObtenerCuenta(int idPersona);
    Task<MovimientosEntity> InsertarMovimiento(MovimientosEntity movimientos);
    Task<MovimientosEntity> ObtenerMovimiento(int PersonaId);
    Task<CuentaEntity> ActualizarCuenta(PersonaUpdateDTO personaDto, CodigoPersonaRequest codigoPersona);
    Task<MovimientosEntity> ActualizarMovimiento(ClienteUpdateDTO clienteDto);
    Task<bool> ExisteCuenta(int codigoPersona);
    Task<bool> ExisteMovimiento(int codigoCliente);
    Task<bool> TieneCuenta(int codigoPersona, string tipoCuenta);
    Task<bool> EliminarCuenta(int codigoPersona);
    Task<bool> EliminarMovimiento(int codigoPersona);

}