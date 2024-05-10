using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Interfaces;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Database.Helpers;

namespace WebApi.Infrastructure.Repositories;

public class ClientePersonaRepository : IClientePersonaRepository
{
    private readonly IAppDb _appDb;
    private readonly IMapper _mapper;

    public ClientePersonaRepository(IAppDb appDb, IMapper mapper)
    {
        _appDb = appDb;
        _mapper = mapper;
    }

    public async Task<ClienteEntity> InsertarCliente(ClienteEntity cliente)
    {
        var dbContext = _appDb.OracleDbContext;
        cliente.Contraseña = EncryptPass(cliente.Contraseña);
        dbContext.Cliente.Add(cliente);
        await dbContext.SaveChangesAsync();
        return cliente;
    }

    public async Task<PersonaEntity> InsertarPersona(PersonaEntity persona)
    {
        var dbContext = _appDb.OracleDbContext;
        dbContext.Persona.Add(persona);
        await dbContext.SaveChangesAsync();
        return persona;
    }

    public async Task<ClienteEntity> ObtenerCliente(int PersonaId)
    {
        var result = await _appDb.OracleDbContext
        .Cliente
        .Where(p => p.PersonaId == PersonaId)
        .FirstOrDefaultAsync();

        return result;
    }

    public async Task<PersonaEntity> ObtenerPersona(int codigoPersona)
    {
        var result = await _appDb.OracleDbContext
            .Persona
            .Where(p => p.IdPersona == codigoPersona)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            result = new PersonaEntity();
        }
        return result;
    }

    public async Task<PersonaUpdateDTO> ActualizarPersona(PersonaUpdateDTO personaDto, int codigoPersona)
    {
        var datosPersona = await ObtenerPersona(codigoPersona);
        if (datosPersona == null)
        {
            throw new ReglaNegociosException("No se encontró la persona.", ErrorType.PERSONA_NO_EXISTE);
        }

        var tipoDatosPersona = datosPersona.GetType();
        var propiedades = tipoDatosPersona.GetProperties();
        bool hayCambios = false;

        foreach (var propiedad in propiedades)
        {
            var valorDto = tipoDatosPersona.GetProperty(propiedad.Name)?.GetValue(_mapper.Map<PersonaEntity>(personaDto));
            var valorDatosPersona = propiedad.GetValue(datosPersona);

            if (valorDto != null && !valorDto.Equals(valorDatosPersona))
            {
                if (propiedad.Name == "IdPersona")
                {
                    continue;
                }
                propiedad.SetValue(datosPersona, valorDto);
                hayCambios = true;
            }
        }

        if (hayCambios)
        {
            _appDb.OracleDbContext.Persona.Update(datosPersona).State = EntityState.Modified;
            await _appDb.OracleDbContext.SaveChangesAsync();
            return _mapper.Map<PersonaUpdateDTO>(datosPersona);
        }

        throw new ReglaNegociosException("No se han encontrado cambios.", ErrorType.SIN_CAMBIOS);
    }

    public async Task<ClienteEntity> ActualizarCliente(ClienteUpdateDto clienteDto)
    {
        var datosCliente = await ObtenerCliente(clienteDto.PersonaId);
        if (datosCliente == null)
        {
            throw new ReglaNegociosException("La persona ingresada no cuenta con una cuenta.", ErrorType.SIN_USUARIO);
        }

        clienteDto.Contraseña = EncryptPass(clienteDto.Contraseña);
        var clienteUpdated = _mapper.Map<ClienteEntity>(clienteDto);
        var tipoDatosCliente = datosCliente.GetType();
        var propiedades = tipoDatosCliente.GetProperties();
        bool hayCambios = false;

        foreach (var propiedad in propiedades)
        {
            var valorActual = propiedad.GetValue(datosCliente);
            var valorNuevo = tipoDatosCliente.GetProperty(propiedad.Name)?.GetValue(clienteUpdated);

            if (valorNuevo != null && !valorNuevo.Equals(valorActual))
            {
                if (propiedad.Name == "IdCliente") // Asumiendo que no quieres actualizar el ID
                {
                    continue;
                }
                propiedad.SetValue(datosCliente, valorNuevo);
                hayCambios = true;
            }
        }

        if (hayCambios)
        {
            _appDb.OracleDbContext.Cliente.Update(datosCliente).State = EntityState.Modified;
            await _appDb.OracleDbContext.SaveChangesAsync();
            return datosCliente;
        }

        throw new ReglaNegociosException("No se han encontrado cambios.", ErrorType.SIN_CAMBIOS);
    }

    public async Task<bool> ExisteCliente(int codigoCliente)
    {
        var existeCliente = await _appDb.OracleDbContext.Cliente.Where(p => p.IdCliente == codigoCliente).FirstOrDefaultAsync();
        return existeCliente != null;
    }

    public async Task<bool> TieneUsuario(int codigoPersona)
    {
        var tieneCuenta = await _appDb.OracleDbContext.Cliente.Where(p => p.PersonaId == codigoPersona).FirstOrDefaultAsync();
        return tieneCuenta != null;
    }

    public async Task<bool> ValidarPassword(ClientePassword parametros)
    {
        var datosCliente = await _appDb.OracleDbContext.Cliente.Where(p => p.PersonaId == parametros.Id).FirstOrDefaultAsync();
        var passCorrecto = false;
        if (datosCliente != null)
        {
            passCorrecto = VerifyPassword(parametros.ContraseñaAnterior, datosCliente.Contraseña);
        }
        return passCorrecto;
    }

    public async Task<bool> ExistePersona(int codigoPersona)
    {
        var existePersona = await _appDb.OracleDbContext.Persona.Where(p => p.IdPersona == codigoPersona).FirstOrDefaultAsync();
        return existePersona != null;
    }

    public async Task<bool> ExisteIdentificacion(string identificacion)
    {
        var existePersona = await _appDb.OracleDbContext.Persona.Where(p => p.Identificacion == identificacion).FirstOrDefaultAsync();
        return existePersona != null;
    }

    private string EncryptPass(string contraseña)
    {
        PasswordHasher passwordHasher = new PasswordHasher();
        return passwordHasher.HashPassword(contraseña);
    }

    private bool VerifyPassword(string password, string encryptedPassword)
    {
        PasswordHasher passwordHasher = new PasswordHasher();
        return passwordHasher.VerifyPassword(password, encryptedPassword);
    }

    public async Task<bool> EliminarPersona(int codigoPersona)
    {
        var dbContext = _appDb.OracleDbContext;
        var persona = await dbContext.Persona.FindAsync(codigoPersona);
        if (persona == null)
        {
            return false;
        }
        dbContext.Persona.Remove(persona);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarCliente(int codigoPersona)
    {
        var dbContext = _appDb.OracleDbContext;
        var cliente = await dbContext.Cliente.FirstOrDefaultAsync(c => c.PersonaId == codigoPersona);
        if (cliente == null)
        {
            return false;
        }
        dbContext.Cliente.Remove(cliente);
        await dbContext.SaveChangesAsync();
        return true;
    }
}