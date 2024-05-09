using AutoMapper;
using Microsoft.Extensions.Logging;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Interfaces;


namespace WebApi.Core.Services;

public class MovimientosService : IMovimientosService
{
    private readonly IMovimientosRepository _movimientosRepository;
    private readonly IClientePersonaRepository _clientePersonaRepository;
    private readonly ILogger<MovimientosService> _logger;
    private readonly IMapper _mapper;

    public MovimientosService(IMovimientosRepository movimientosRepository, 
        ILogger<MovimientosService> logger, IMapper mapper, IClientePersonaRepository clientePersonaRepository)
    {
        _movimientosRepository = movimientosRepository;
        _logger = logger;
        _mapper = mapper;
        _clientePersonaRepository = clientePersonaRepository;
    }
    public async Task<CuentaEntity> InsertarCuenta(CuentaRequest cuenta)
    {
        var existeCliente = await _clientePersonaRepository.ExisteCliente(cuenta.IdCliente);
        if (!existeCliente)
        {
            _logger.LogInformation("El cliente ingresado no existe {@cuenta.IdCliente}", cuenta.IdCliente);
            throw new ReglaNegociosException("El cliente ingresado no existe.", ErrorType.CLIENTE_NO_EXISTE);
        }
        var request = _mapper.Map<CuentaEntity>(cuenta);
        var tieneCuenta = await _movimientosRepository.TieneCuenta(cuenta.IdCliente, cuenta.TipoCuenta);
        if (tieneCuenta)
        {
            _logger.LogInformation("Esta persona ya tiene una cuenta del mismo tipo. {@cuenta}", cuenta);
            throw new ReglaNegociosException("Esta persona ya tiene una cuenta del mismo tipo. Intente con otro tipo de cuenta", ErrorType.CUENTA_DUPLICADA);
        }
        if (request != null)
        {
            _logger.LogInformation("Iniciando inserción de datos en tabla CLIENTE {@request}", request);
            return await _movimientosRepository.InsertarCuenta(request);
        }
        else
        {
            _logger.LogInformation("Ingrese el id la persona {@request}", request);
            throw new ReglaNegociosException("Ingrese el id de la persona.", ErrorType.validacion_parametro_entrada);
        }
    }

    public Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest cliente)
    {
        throw new NotImplementedException();
    }
    public Task<CuentaEntity> ActualizarCuenta(CuentaRequest cuentaUpdate, CodigoPersonaRequest codigoPersona)
    {
        throw new NotImplementedException();
    }

    public Task<MovimientosEntity> ActualizarMovimiento(int PersonaId, ClienteUpdateRequest clienteUpdate, string passwordAnterior)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EliminarCuenta(int codigoPersona)
    {
        throw new NotImplementedException();
    }

    public Task<bool> EliminarMovimiento(int codigoCliente)
    {
        throw new NotImplementedException();
    }
    public Task<CuentaEntity> ObtenerCuenta(int codigoCuenta)
    {
        throw new NotImplementedException();
    }

    public Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento)
    {
        throw new NotImplementedException();
    }
}