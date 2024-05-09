using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.ComponentModel;
using System.Threading.Tasks;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Contracts.Responses;
using WebApi.Core.Interfaces;
using WebApi.Core.Services;
using WebApi.WebApi.Controllers;
using WebApi.WebApi.Models;

namespace WebApi.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/api")]
    [ApiController]
    [OpenApiTag("MovimientosController", Description = "Controlador de Endpoints para Movimientos y Cuentas.")]
    public class MovimientosController : BaseApiController
    {
        private readonly ILogger<MovimientosController> _logger;
        private readonly IMovimientosService _movimientosService;

        public MovimientosController(ILogger<MovimientosController> logger,
        IMovimientosService movimientosService)
        {
            _logger = logger;
            _movimientosService = movimientosService;
        }
    }

    [HttpPost("movimiento")]
    [OpenApiOperation("InsertarMovimiento", description: "Inserta los datos de la persona en la tabla Persona.")]
    [SwaggerResponse(StatusCodes.Status201Created, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve una lista de las personas que fueron insertadas en la base de datos.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> InsertarMovimiento([Description("Datos de la persona a insertar en tabla Persona")][FromBody] PersonaRequest persona)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de InsertarPersona {@persona}", persona);
            var result = await _clientePersonaService.InsertarPersona(persona);

            if (!string.IsNullOrWhiteSpace(result.Nombre))
            {
                _logger.LogInformation("Los datos de las personas se han insertado con exito {@persona}", persona);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            else
            {
                _logger.LogInformation("Ha ocurrido un error  al insertar los datos de las personas {@persona}", persona);
                return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al insertar los datos de las personas.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al insertar los datos de las personas {@persona}", persona);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de las personas {@persona}", persona);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.error_interno_servidor,
                ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
            });
        }
    }
}
