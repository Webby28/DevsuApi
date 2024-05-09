using AutoMapper;
using Core.Contracts.Enums;
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

    public async Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest movimientos)
    {
        int saldoRestante = 0;
        int saldoActual = 0;
        var existeCuenta = await _movimientosRepository.ExisteCuenta(movimientos.NumeroCuenta);
        if (existeCuenta)
        {
            saldoActual = await _movimientosRepository.SaldoActual(movimientos.NumeroCuenta);
            if (saldoActual < 0)
            {
                _logger.LogInformation("La operación excedió el saldo disponible. {@movimientos}", movimientos);
                throw new ReglaNegociosException("Saldo no disponible.", ErrorType.SALDO_EXCEDIDO);
            }
            switch (movimientos.TipoMovimiento)
            {
                case TipoMovimiento.DEPOSITO:
                    saldoRestante = saldoActual + movimientos.Valor;
                    break;

                case TipoMovimiento.RETIRO:
                    saldoRestante = saldoActual - movimientos.Valor;
                    break;
            }
            if (saldoRestante < 0)
            {
                _logger.LogInformation("La operación excedió el saldo disponible. {@movimientos}", movimientos);
                throw new ReglaNegociosException("Saldo no disponible.", ErrorType.SALDO_EXCEDIDO);
            }
            else
            {
                _logger.LogInformation("Se actualiza el saldo disponible en la cuenta. {@movimientos}", movimientos);
                var request = _mapper.Map<MovimientosEntity>(movimientos);
                request.Saldo = saldoRestante;
                request.FechaRegistro = DateTime.Now;
                return await _movimientosRepository.InsertarMovimiento(request);
            }
        }
        else
        {
            throw new ReglaNegociosException("No existe la cuenta ingresada. Intente con nuevamente.", ErrorType.CUENTA_NO_EXISTE);
        }
    }

    public async Task<CuentaEntity> ActualizarCuenta(CuentaRequest cuentaUpdate, int codigoPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<MovimientosEntity> ActualizarMovimiento(MovimientosRequest movimientoUpdate, int codigoMovimiento)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EliminarCuenta(int codigoPersona)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> EliminarMovimiento(int codigoCliente)
    {
        throw new NotImplementedException();
    }

    public async Task<CuentaEntity> ObtenerCuenta(int codigoCuenta)
    {
        _logger.LogInformation("Inicia operacion para consultar existencia de cuenta en tabla Cuenta {@codigoCuenta}", codigoCuenta);
        return await _movimientosRepository.ObtenerCuenta(codigoCuenta);
    }

    public async Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento)
    {
        _logger.LogInformation("Inicia operacion para consultar existencia de cuenta en tabla Cuenta {@codigoCuenta}", codigoMovimiento);
        return await _movimientosRepository.ObtenerMovimiento(codigoMovimiento);
    }
}