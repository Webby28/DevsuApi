using AutoMapper;
using Core.Contracts.Helpers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using System.Text;
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
        var existeCuenta = await _movimientosRepository.ExisteCuenta(cuenta.NumeroCuenta);
        if (existeCuenta)
        {
            _logger.LogInformation("El número de cuenta proporcionado ya ha sido ingresado. {@Cuenta.NumeroCuenta}", cuenta.NumeroCuenta);
            throw new ReglaNegociosException("El número de cuenta proporcionado ya ha sido ingresado. Intente nuevamente.", ErrorType.CUENTA_DUPLICADA);
        }

        var existeCliente = await _clientePersonaRepository.ExisteCliente(cuenta.IdCliente);
        if (!existeCliente)
        {
            _logger.LogInformation("El cliente ingresado no existe {@Cuenta.IdCliente}", cuenta.IdCliente);
            throw new ReglaNegociosException("El cliente ingresado no existe.", ErrorType.CLIENTE_NO_EXISTE);
        }

        var tieneCuenta = await _movimientosRepository.TieneCuenta(cuenta.IdCliente, cuenta.TipoCuenta);
        if (tieneCuenta)
        {
            _logger.LogInformation("Esta persona ya tiene una cuenta del mismo tipo. {@Cuenta}", cuenta);
            throw new ReglaNegociosException("Esta persona ya tiene una cuenta del mismo tipo. Intente con otro tipo de cuenta", ErrorType.CUENTA_DUPLICADA);
        }

        var request = _mapper.Map<CuentaEntity>(cuenta);
        _logger.LogInformation("Iniciando inserción de datos en tabla cliente {@Request}", request);
        return await _movimientosRepository.InsertarCuenta(request);
    }


    public async Task<MovimientosEntity> InsertarMovimiento(MovimientosRequest movimiento)
    {
        int saldoRestante = 0;
        var existeCuenta = await _movimientosRepository.ExisteCuenta(movimiento.NumeroCuenta);
        if (existeCuenta)
        {
            int saldoActual = await _movimientosRepository.SaldoActual(movimiento.NumeroCuenta);
            if (saldoActual < 0)
            {
                _logger.LogInformation("La operación excedió el saldo disponible. {@Movimientos}", movimiento);
                throw new ReglaNegociosException("Saldo no disponible.", ErrorType.SALDO_EXCEDIDO);
            }
            switch (movimiento.TipoMovimiento)
            {
                case '0':
                    saldoRestante = saldoActual + movimiento.Valor;
                    break;

                case '1':
                    saldoRestante = saldoActual - movimiento.Valor;
                    break;
            }
            if (saldoRestante < 0)
            {
                _logger.LogInformation("La operación excedió el saldo disponible. {@Movimientos}", movimiento);
                throw new ReglaNegociosException("Saldo no disponible.", ErrorType.SALDO_EXCEDIDO);
            }
            else
            {
                _logger.LogInformation("Se actualiza el saldo disponible en la cuenta. {@Movimientos}", movimiento);
                var request = _mapper.Map<MovimientosEntity>(movimiento);
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

    public async Task<CuentaEntity> ActualizarCuenta(CuentaUpdateDto cuentaUpdate, int numeroCuenta)
    {
        _logger.LogInformation("Se verifica si existe cuenta. {@NumeroCuenta}", numeroCuenta);
        var existeCuenta = await _movimientosRepository.ExisteCuenta(numeroCuenta);
        if (existeCuenta)
        {
            var updateCuenta = _mapper.Map<CuentaUpdateDto>(cuentaUpdate);
            _logger.LogInformation("Se procede a actualizar el registro. {@NumeroCuenta} {CuentaUpdate}", numeroCuenta, cuentaUpdate);
            return await _movimientosRepository.ActualizarCuenta(updateCuenta, numeroCuenta);
        }
        else
        {
            throw new ReglaNegociosException("Persona no existe", ErrorType.PERSONA_NO_EXISTE);
        }
    }

    public async Task<MovimientosEntity> ActualizarMovimiento(MovimientoUpdateDto movimientoUpdate, int codigoMovimiento)
    {
        _logger.LogInformation("Se verifica si existe el movimiento. {@CodigoMovimiento}", codigoMovimiento);
        var existeMovimiento = await _movimientosRepository.ExisteMovimiento(codigoMovimiento);
        if (existeMovimiento)
        {
            _logger.LogInformation("Se procede a actualizar el registro. {@CodigoMovimiento} {MovimientoUpdate}", codigoMovimiento, movimientoUpdate);
            var updateCuenta = _mapper.Map<MovimientoUpdateDto>(movimientoUpdate);
            return await _movimientosRepository.ActualizarMovimiento(updateCuenta, codigoMovimiento);
        }
        else
        {
            throw new ReglaNegociosException("Persona no existe", ErrorType.PERSONA_NO_EXISTE);
        }
    }

    public async Task<bool> EliminarCuenta(int numeroCuenta)
    {
        var existeCuenta = await _movimientosRepository.ExisteCuenta(numeroCuenta);
        if (existeCuenta)
        {
            var conMovimiento = await _movimientosRepository.CuentaConMovimiento(numeroCuenta);
            if (!conMovimiento)
            {
                return await _movimientosRepository.EliminarCuenta(numeroCuenta);
            }
            else
            {
                throw new ReglaNegociosException("Cuenta con movimientos.", ErrorType.CUENTA_CON_MOVIMIENTOS);
            }
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> EliminarMovimiento(int codigoMovimiento)
    {
        var existeMovimiento = await _movimientosRepository.ExisteMovimiento(codigoMovimiento);
        if (existeMovimiento)
        {
            return await _movimientosRepository.EliminarMovimiento(codigoMovimiento);
        }
        else
        {
            return false;
        }
    }

    public async Task<CuentaEntity> ObtenerCuenta(int codigoCuenta)
    {
        _logger.LogInformation("Inicia operacion para consultar existencia de cuenta en tabla Cuenta {@CodigoCuenta}", codigoCuenta);
        return await _movimientosRepository.ObtenerCuenta(codigoCuenta);
    }

    public async Task<MovimientosEntity> ObtenerMovimientos(int codigoMovimiento)
    {
        _logger.LogInformation("Inicia operacion para consultar existencia de cuenta en tabla Cuenta {@CodigoCuenta}", codigoMovimiento);
        return await _movimientosRepository.ObtenerMovimiento(codigoMovimiento);
    }

    public async Task<byte[]> GenerarReporte(string rangoFechas, int codigoCliente)
    {
        var existeCliente = await _clientePersonaRepository.ExisteCliente(codigoCliente);
        if (!existeCliente)
        {
            throw new ReglaNegociosException("El código de cliente no existe.", ErrorType.CLIENTE_NO_EXISTE);
        }
        else
        {
            var tieneMovimiento = await _movimientosRepository.TieneMovimiento(codigoCliente);
            if (!tieneMovimiento)
            {
                throw new ReglaNegociosException("El cliente no tiene movientos.", ErrorType.SIN_MOVIMIENTOS);
            }

            var partesRango = rangoFechas.Split('|');
            if (partesRango.Length != 2)
            {
                throw new ArgumentException("El formato del rango de fechas es incorrecto.");
            }
            DateTime desde = DateTime.Parse(partesRango[0]);
            DateTime hasta = DateTime.Parse(partesRango[1]);
            var reportData = await _movimientosRepository.ObtenerMovimientoPorFecha(codigoCliente, desde, hasta);

            // Generar el HTML del reporte utilizando los datos obtenidos
            string htmlReport = GenerarHTMLReporte(reportData);

            // Generar el PDF del reporte a partir del HTML utilizando la clase PdfGenerator
            var pdfGenerator = new PdfGenerator();
            byte[] pdfBytes = pdfGenerator.GeneratePdf(htmlReport);

            return pdfBytes;
        }
    }

    private static string GenerarHTMLReporte(IEnumerable<MovimientosEntity> reportData)
    {
        // Comienza la construcción del HTML
        var htmlBuilder = new StringBuilder();
        htmlBuilder.AppendLine("<!DOCTYPE html>");
        htmlBuilder.AppendLine("<html>");
        htmlBuilder.AppendLine("<head>");
        htmlBuilder.AppendLine("<title>Reporte de Movimientos</title>");
        htmlBuilder.AppendLine("</head>");
        htmlBuilder.AppendLine("<body>");
        htmlBuilder.AppendLine("<h1>Reporte de Movimientos</h1>");

        // Verifica si hay datos en el reportData
        if (reportData?.Any() == true)
        {
            // Agrega la tabla con los encabezados
            htmlBuilder.AppendLine("<table>");
            htmlBuilder.AppendLine("<thead>");
            htmlBuilder.AppendLine("<tr>");
            htmlBuilder.AppendLine("<th>ID Movimiento</th>");
            htmlBuilder.AppendLine("<th>Fecha</th>");
            htmlBuilder.AppendLine("<th>Detalle</th>");
            htmlBuilder.AppendLine("</tr>");
            htmlBuilder.AppendLine("</thead>");
            htmlBuilder.AppendLine("<tbody>");

            // Agrega las filas con los datos de los movimientos
            foreach (var movimiento in reportData)
            {
                htmlBuilder.AppendLine("<tr>");
                htmlBuilder.AppendLine($"<td>{movimiento.IdMovimiento}</td>");
                htmlBuilder.AppendLine($"<td>{movimiento.Fecha}</td>");
                htmlBuilder.AppendLine($"<td>{movimiento.Valor}</td>");
                htmlBuilder.AppendLine($"<td>{movimiento.Saldo}</td>");
                htmlBuilder.AppendLine($"<td>{movimiento.NumeroCuenta}</td>");
                htmlBuilder.AppendLine("</tr>");
            }

            htmlBuilder.AppendLine("</tbody>");
            htmlBuilder.AppendLine("</table>");
        }
        else
        {
            // Si no hay datos, muestra un mensaje indicando que no se encontraron movimientos
            htmlBuilder.AppendLine("<p>No se encontraron movimientos para mostrar.</p>");
        }

        // Finaliza la construcción del HTML
        htmlBuilder.AppendLine("</body>");
        htmlBuilder.AppendLine("</html>");

        return htmlBuilder.ToString();
    }
    /// <summary>
    /// Método que permite la actulizacion del estado de un subservicio favorito luego de que haya sido pagado.
    /// </summary>
    /// <param name="estado">Corresponde al identificador del subServicio.</param>
    /// <param name="id">Corresponde a los datos para el pago del subServicio.</param>
    /// <param name="codigoCliente">Corresponde al identificador del cliente.</param>
    /// <returns>Obtenemos el resultado de operación ejecutada.</returns>
    public async Task<int> ActualizarEstado(char estado, int id, Tabla tabla)
    {
        if(tabla.Equals(Tabla.CUENTA) || tabla.Equals(Tabla.MOVIMIENTO))
        {
            var des = await _movimientosRepository.ActualizarEstado(estado, id, tabla);
            return des;
        }
        else
        {
            throw new ReglaNegociosException("Operación seleccionada no corresponde a movimientos.", ErrorType.VALIDACION_PARAMETROS_ENTRADA);
        }


    }
}