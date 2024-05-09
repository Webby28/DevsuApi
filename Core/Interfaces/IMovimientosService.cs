using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosService
{
    Task<PersonaEntity> InsertarCuenta(PersonaRequest cuenta);
    Task<ClienteEntity> InsertarMovimiento(ClienteRequest cliente);
    Task<PersonaEntity> ObtenerCuenta(CodigoPersonaRequest codigoPersona);
    Task<ClienteEntity> ObtenerMovimiento(int codigoCliente);
    Task<PersonaUpdateDTO> ActualizarCuenta(PersonaRequest personaUpdate, CodigoPersonaRequest codigoPersona);
    Task<ClienteEntity> ActualizarMovimiento(int PersonaId, ClienteUpdateRequest clienteUpdate, string passwordAnterior);
    Task<bool> EliminarCuenta(int codigoPersona);
    Task<bool> EliminarMovimiento(int codigoCliente);
}