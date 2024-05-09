using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IClientePersonaService
{
    Task<PersonaEntity> InsertarPersona(PersonaRequest cuenta);

    Task<ClienteEntity> InsertarCliente(ClienteRequest cliente);

    Task<PersonaEntity> ObtenerPersona(CodigoPersonaRequest codigoPersona);

    Task<ClienteEntity> ObtenerCliente(int codigoCliente);

    Task<PersonaUpdateDTO> ActualizarPersona(PersonaRequest personaUpdate, CodigoPersonaRequest codigoPersona);

    Task<ClienteEntity> ActualizarCliente(int PersonaId, ClienteUpdateRequest clienteUpdate, string passwordAnterior);

    Task<bool> EliminarPersona(int codigoPersona);

    Task<bool> EliminarCliente(int codigoCliente);
}