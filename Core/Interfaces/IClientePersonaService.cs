using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IClientePersonaService
{
    Task<PersonaEntity> InsertarPersona(PersonaRequest persona);

    Task<ClienteEntity> InsertarCliente(ClienteRequest cliente);

    Task<PersonaEntity> ObtenerPersona(int codigoPersona);

    Task<ClienteEntity> ObtenerCliente(int codigoCliente);

    Task<PersonaUpdateDto> ActualizarPersona(PersonaRequest personaUpdate, int codigoPersona);

    Task<ClienteEntity> ActualizarCliente(int PersonaId, ClienteUpdateRequest clienteUpdate, string passwordAnterior);

    Task<bool> EliminarPersona(int codigoPersona);

    Task<bool> EliminarCliente(int codigoCliente);
    Task<int> ActualizarEstado(string estado, int id, Tabla pERSONA);
}