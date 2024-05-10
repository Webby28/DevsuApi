using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IClientePersonaRepository
{
    Task<PersonaEntity> InsertarPersona(PersonaEntity personas);

    Task<PersonaEntity> ObtenerPersona(int codigoPersona);

    Task<ClienteEntity> InsertarCliente(ClienteEntity cliente);

    Task<ClienteEntity> ObtenerCliente(int PersonaId);

    Task<PersonaUpdateDTO> ActualizarPersona(PersonaUpdateDTO personaDto, int codigoPersona);

    Task<ClienteEntity> ActualizarCliente(ClienteUpdateDTO clienteDto);

    Task<bool> ExistePersona(int codigoPersona);

    Task<bool> ExisteCliente(int codigoCliente);

    Task<bool> ExisteIdentificacion(string identificacion);

    Task<bool> TieneUsuario(int codigoPersona);

    Task<bool> ValidarPassword(ClientePassword parametros);

    Task<bool> EliminarPersona(int codigoPersona);

    Task<bool> EliminarCliente(int codigoPersona);
}