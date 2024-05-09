
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Contracts.Responses;
using WebApi.Core.Interfaces;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Database.Helpers;

namespace WebApi.Infrastructure.Repositories;

public class MovimientosRepository : IMovimientosRepository
{
    private readonly IAppDb _appDb;
    private readonly IMapper _mapper;

    public MovimientosRepository(IAppDb appDb, IMapper mapper)
    {
        _appDb = appDb;
        _mapper = mapper;
    }
    public async Task<CuentaEntity> InsertarCuenta(CuentaEntity cuenta)
    {
        var dbContext = _appDb.OracleDbContext;
        dbContext.Cuenta.Add(cuenta);
        await dbContext.SaveChangesAsync();
        return cuenta;
    }

    public async Task<MovimientosEntity> InsertarMovimiento(MovimientosEntity movimientos)
    {
        var dbContext = _appDb.OracleDbContext;
        dbContext.Movimientos.Add(movimientos);
        await dbContext.SaveChangesAsync();
        return movimientos;
    }
    public async Task<CuentaEntity> ActualizarCuenta(PersonaUpdateDTO personaDto, CodigoPersonaRequest codigoPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<MovimientosEntity> ActualizarMovimiento(ClienteUpdateDTO clienteDto)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EliminarCuenta(int codigoPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EliminarMovimiento(int codigoPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExisteCuenta(int codigoPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> ExisteMovimiento(int codigoCliente)
    {
        throw new NotImplementedException();
    }
    public async Task<CuentaEntity> ObtenerCuenta(int idPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<MovimientosEntity> ObtenerMovimiento(int PersonaId)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> TieneCuenta(int codigoCliente, string tipoCuenta)
    {
        var tieneCuenta = await _appDb.OracleDbContext.Cuenta
            .AnyAsync(p => p.IdCliente == codigoCliente && p.TipoCuenta == tipoCuenta);

        return tieneCuenta;
    }
}