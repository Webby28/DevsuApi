using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Core.Interfaces;

public interface IMovimientosRepository
{
    Task<PersonaEntity> InsertarCuenta(PersonaEntity personas);
    Task<PersonaEntity> ObtenerCuenta(CodigoPersonaRequest idPersona);
    Task<ClienteEntity> InsertarMovimiento(ClienteEntity cliente);
    Task<ClienteEntity> ObtenerMovimiento(int PersonaId);
    Task<PersonaUpdateDTO> ActualizarCuenta(PersonaUpdateDTO personaDto, CodigoPersonaRequest codigoPersona);
    Task<ClienteEntity> ActualizarMovimiento(ClienteUpdateDTO clienteDto);
    Task<bool> ExisteCuenta(int codigoPersona);
    Task<bool> ExisteMovimiento(int codigoCliente);
    Task<bool> ExisteIdentificacion(string identificacion);
    Task<bool> TieneCuenta(int codigoPersona);
    Task<bool> ValidarPassword(ClientePassword parametros);
    Task<bool> EliminarCuenta(int codigoPersona);
    Task<bool> EliminarMovimiento(int codigoPersona);

}