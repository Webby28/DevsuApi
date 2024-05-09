﻿using AutoMapper;
using Microsoft.Extensions.Logging;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Interfaces;


namespace WebApi.Core.Services;

public class ClientePersonaService : IClientePersonaService
{
    private readonly IClientePersonaRepository _clientePersonaRepository;
    private readonly ILogger<ClientePersonaService> _logger;
    private readonly IMapper _mapper;

    public ClientePersonaService(IClientePersonaRepository clientePersonaRepository,
        ILogger<ClientePersonaService> logger, IMapper mapper)
    {
        _clientePersonaRepository = clientePersonaRepository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ClienteEntity> InsertarCliente(ClienteRequest cliente)
    {
        var tieneCuenta = await _clientePersonaRepository.TieneCuenta(cliente.PersonaId);
        if (tieneCuenta)
        {
            _logger.LogInformation("El persona ya tiene cuenta {@request}", cliente.PersonaId);
            throw new ReglaNegociosException("La persona ingresada ya tiene una cuenta registrada.", ErrorType.datos_duplicados);
        }
        var request = _mapper.Map<ClienteEntity>(cliente);
        var existePersona = await _clientePersonaRepository.ExistePersona(cliente.PersonaId);
        if (!existePersona)
        {
            _logger.LogInformation("El persona ingresada no existe. {@request}", cliente.PersonaId);
            throw new ReglaNegociosException("La persona ingresada no existe.", ErrorType.codigo_persona_no_existe);
        }
        if (request.PersonaId != 0)
        {
            _logger.LogInformation("Iniciando inserción de datos en tabla CLIENTE {@request}", request);
            return await _clientePersonaRepository.InsertarCliente(request);
        }
        else
        {
            _logger.LogInformation("Ingrese el id la persona {@request}", request);
            throw new ReglaNegociosException("Ingrese el id de la persona.", ErrorType.validacion_parametro_entrada);
        }
    }

    public async Task<PersonaEntity> InsertarPersona(PersonaRequest persona)
    {

        var existeIdentificacion = await _clientePersonaRepository.ExisteIdentificacion(persona.Identificacion);
        if (existeIdentificacion)
        {
            _logger.LogInformation("Identificacion duplicada {@request}", persona.Identificacion);
            throw new ReglaNegociosException("El numero de identificacion ya ha sido registrado con anterioridad", ErrorType.datos_duplicados);
        }
        var request = _mapper.Map<PersonaEntity>(persona);

        if (request.Nombre != null)
        {
            _logger.LogInformation("Iniciando inserción de datos en tabla PERSONA {@request}", request);
            return await _clientePersonaRepository.InsertarPersona(request);
        }
        else
        {
            _logger.LogInformation("Solicitud inválida. {@request}", request);
            return  new PersonaEntity();
        }
    }

    public async Task<PersonaEntity> ObtenerPersona(CodigoPersonaRequest codigoPersona)
    {
        _logger.LogInformation("Inicia operacion para consultar existencia de persona en tabla PERSONA {@codigoCliente}", codigoPersona);
        return await _clientePersonaRepository.ObtenerPersona(codigoPersona);
    }
    public async Task<ClienteEntity> ObtenerCliente(int codigoCliente)
    {
        _logger.LogInformation("Inicia operacion para consultar existencia de persona en tabla CLIENTE {@codigoCliente}", codigoCliente);
        return await _clientePersonaRepository.ObtenerCliente(codigoCliente);
    }

    public async Task<PersonaUpdateDTO> ActualizarPersona(PersonaRequest personaUpdate, CodigoPersonaRequest codigoPersona)
    {
        var existePersona = await _clientePersonaRepository.ExistePersona(codigoPersona.PersonaId);
        var updatePersona = _mapper.Map<PersonaUpdateDTO>(personaUpdate);
        if (existePersona)
        {
            
            return await _clientePersonaRepository.ActualizarPersona(updatePersona, codigoPersona);

        }
        else
        {
            throw new ReglaNegociosException("Persona no existe", ErrorType.codigo_persona_no_existe);

        }       
    }

    public async Task<ClienteEntity> ActualizarCliente(int PersonaId, ClienteUpdateRequest clienteUpdate, string passwordAnterior)
    {
        var existeCuenta = await _clientePersonaRepository.TieneCuenta(PersonaId);
        if(clienteUpdate.Estado != "A") {
            throw new ReglaNegociosException("La cuenta no se encuentra activa. Contacte con su gestor.", ErrorType.cuenta_no_activa);
        }
        else
        {
            if (existeCuenta)
            {
                var contraseñaCorrecta = await _clientePersonaRepository.ValidarPassword(new ClientePassword() { Id = PersonaId, ContraseñaAnterior = passwordAnterior });
                var updateCliente = _mapper.Map<ClienteUpdateDTO>(clienteUpdate);
                if (contraseñaCorrecta)
                {
                    updateCliente.PersonaId = PersonaId;
                    return await _clientePersonaRepository.ActualizarCliente(updateCliente);

                }
                else
                {
                    throw new ReglaNegociosException("La contraseña anterior ingresada es incorrecta.", ErrorType.contraseña_incorrecta);
                }
            }
            else
            {
                throw new ReglaNegociosException("Persona no tiene una cuenta", ErrorType.codigo_persona_no_existe);
            }
        }
        

    }

    public async Task<bool> EliminarPersona(int codigoPersona)
    {
        var existePersona = await _clientePersonaRepository.ExistePersona(codigoPersona);
        if (existePersona)
        {
            return await _clientePersonaRepository.EliminarPersona(codigoPersona);
        }
        else
        {
            throw new ReglaNegociosException("No existe persona a eliminar", ErrorType.codigo_persona_no_existe);
        }
    }

    public async Task<bool> EliminarCliente(int codigoCliente)
    {
        var existePersona = await _clientePersonaRepository.TieneCuenta(codigoCliente);
        if (existePersona)
        {
            return await _clientePersonaRepository.EliminarCliente(codigoCliente);
        }
        else
        {
            throw new ReglaNegociosException("No existe cliente a eliminar", ErrorType.codigo_persona_no_existe);
        }
    }  
}