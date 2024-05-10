using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.ComponentModel;
using System.Threading.Tasks;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Contracts.Responses;
using WebApi.Core.Interfaces;
using WebApi.Models;

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

        [HttpPost("cuenta")]
        [OpenApiOperation("InsertarCuenta", description: "Crea una cuenta para un cliente.")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(CuentaResponse), Description = "Operación exitosa. Devuelve la cuenta creada.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> InsertarCuenta([Description("Datos a insertar en tabla Cuenta")][FromBody] CuentaRequest cuenta)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de InsertarCuenta {@cuenta}", cuenta);
                var result = await _movimientosService.InsertarCuenta(cuenta);

                if (!string.IsNullOrWhiteSpace(result.Estado))
                {
                    _logger.LogInformation("La cuenta se ha creado con éxito {@cuenta}", cuenta);
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {
                    _logger.LogInformation("Ha ocurrido un error  al crear la cuenta {@cuenta}", cuenta);
                    return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al crear la cuenta.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al crear la cuenta {@cuenta}", cuenta);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de las personas {@persona}", cuenta);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpPost("movimientos")]
        [OpenApiOperation("InsertarMovimientos", description: "Inserta los datos de los movimientos realizados por una cuenta.")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve el movimiento realizado por la cuenta.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> InsertarMovimientos([Description("Datos a insertar en tabla Cuenta")][FromBody] MovimientosRequest movimiento)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de InsertarPersona {@cuenta}", movimiento);
                var result = await _movimientosService.InsertarMovimiento(movimiento);

                if (result.IdMovimiento != 0)
                {
                    _logger.LogInformation("El movimiento se ha registrado con éxito {@cuenta}", movimiento);
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {
                    _logger.LogInformation("Ha ocurrido un error  al registrar el movimiento {@cuenta}", movimiento);
                    return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al registrar el movimiento.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al registrar el movimiento {@movimiento}", movimiento);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de las personas {@movimiento}", movimiento);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al registrar el movimiento. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpGet("cuenta/{id}")]
        [OpenApiOperation("ObtenerCuenta", description: "Obtiene los datos de una cuenta.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve la cuenta de un cliente.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se ha encontrado la cuenta.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> ObtenerCuenta([Description("Id de la cuenta")][FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de ObtenerCuenta {@id}", id);
                var result = await _movimientosService.ObtenerCuenta(id);

                if (!string.IsNullOrWhiteSpace(result.Estado))
                {
                    _logger.LogInformation("Se lista la cuenta con éxito {@cuenta}", id);
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    _logger.LogInformation("No se ha encontrado la cuenta {@id}", id);
                    return StatusCode(StatusCodes.Status204NoContent, "No se ha encontrado la cuenta.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al crear la cuenta {@id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la obtención de la información de la cuenta {@id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpGet("movimientos/{id}")]
        [OpenApiOperation("ObtenerMovimientos", description: "Obtiene el movimiento de una cuenta.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve el movimiento de una cuenta.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se ha encontrado el movimiento.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> ObtenerMovimientos([Description("Id del movimiento")][FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de ObtenerMovimientos {@id}", id);
                var result = await _movimientosService.ObtenerMovimientos(id);

                if (result.IdMovimiento != 0)
                {
                    _logger.LogInformation("Se lista el movimiento con éxito {@id}", id);
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    _logger.LogInformation("No se ha encontrado el movimiento {@id}", id);
                    return StatusCode(StatusCodes.Status204NoContent, "No se ha encontrado el movimiento.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al mostrar el movimiento {@id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor obre la obtención de la información del movimiento {@id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al mostrar el movimientos. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpPut("cuenta/{id}")]
        [OpenApiOperation("ActualizarCuenta", description: "Actualiza los datos de la cuenta.")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(PersonaResponse), Description = "Operación exitosa. Se han actualizado los datos con éxito.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> ActualizarCuenta([Description("Numero de la cuenta a actualizar")][FromRoute] int id, [Description("Datos a actualizar en tabla Cuenta")][FromBody] CuentaUpdateDTO cuenta)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de ActualizarCuenta {@cuenta}", cuenta);
                var result = await _movimientosService.ActualizarCuenta(cuenta, id);

                if (!string.IsNullOrWhiteSpace(result.Estado))
                {
                    _logger.LogInformation("Se han actualizado los datos con éxito {@cuenta}", cuenta);
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {
                    _logger.LogInformation("Ha ocurrido un error  al actualizar los datos de la cuenta {@cuenta}", cuenta);
                    return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al actualizar la cuenta.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al actualizar la cuenta {@cuenta}", cuenta);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la actualización de los datos de las personas {@persona}", cuenta);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al actualizar los datos de la cuenta. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpPut("movimientos/{id}")]
        [OpenApiOperation("ActualizarMovimientos", description: "Endpoint para actualizar los datos de los movimientos.")]
        [SwaggerResponse(StatusCodes.Status201Created, typeof(PersonaResponse), Description = "Operación exitosa. Se han actualizado los datos con éxito.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> ActualizarMovimientos([Description("Id del movimiento a actualizar")][FromRoute] int id, [Description("Datos a insertar en tabla Cuenta")][FromBody] MovimientoUpdateDTO movimientos)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de InsertarPersona {@movimientos}", movimientos);
                var result = await _movimientosService.ActualizarMovimiento(movimientos, id);

                if (result.IdMovimiento != 0)
                {
                    _logger.LogInformation("Se han actualizado los datos con éxito {@movimientos}", movimientos);
                    return StatusCode(StatusCodes.Status200OK, result);
                }
                else
                {
                    _logger.LogInformation("Ha ocurrido un error  al actualizar el movimiento {@movimientos}", movimientos);
                    return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al actualizar el movimiento.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al actualizar el movimiento {@movimientos}", movimientos);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la actualización de los datos de los movimientos {@movimientos}", movimientos);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al actualizar el movimiento. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpDelete("cuenta/{id}")]
        [OpenApiOperation("EliminarCuenta", description: "Endpoint que elimina la cuenta del cliente.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Se ha eliminado el registro con éxito.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> EliminarCuenta([Description("Cuenta a eliminar")][FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de InsertarPersona {@id}", id);
                var result = await _movimientosService.EliminarCuenta(id);

                if (result)
                {
                    _logger.LogInformation("Se ha eliminado el registro con éxito.{@id}", id);
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {
                    _logger.LogInformation("Ha ocurrido un error  al eliminar la cuenta {@id}", id);
                    return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al eliminar la cuenta.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al eliminar la cuenta {@id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la eliminación de la cuenta {@id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al eliminar la cuenta. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpDelete("movimientos/{id}")]
        [OpenApiOperation("EliminarMovimientos", description: "Endpont que elimina un movimiento.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Se ha eliminado el registro con éxito.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> EliminarMovimientos([Description("Numero de la cuenta a eliminar")][FromRoute] int id)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de InsertarPersona {@id}", id);
                var result = await _movimientosService.EliminarMovimiento(id);

                if (result)
                {
                    _logger.LogInformation("Se ha eliminado el registro con éxito {@id}", id);
                    return StatusCode(StatusCodes.Status201Created, result);
                }
                else
                {
                    _logger.LogInformation("Ha ocurrido un error al eliminar el movimiento {@id}", id);
                    return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al crear la cuenta.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error al eliminar el movimiento {@id}", id);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor sobre la eliminación del movimiento {@id}", id);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error al eliminar el movimiento. Intente nuevamente mas tarde",
                });
            }
        }

        [HttpGet("reportes")]
        [OpenApiOperation("GenerarReporte", description: "Obtiene un reporte de los movimientos de una persona en un rango de fecha.")]
        [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Genera un pdf con los movimientos.")]
        [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se ha encontrado el movimiento.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
        [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
        [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
        public async Task<IActionResult> GenerarReporte([Description("Id del movimiento")][FromQuery] string rangoFechas, [FromQuery] int codigoCliente)
        {
            try
            {
                _logger.LogInformation("Inicio solicitud de GenerarReporte {@RangoFechas} {@CodigoCliente}", rangoFechas, codigoCliente);
                var result = await _movimientosService.GenerarReporte(rangoFechas, codigoCliente);

                if (result != null)
                {
                    _logger.LogInformation("Se lista el movimiento con éxito {@id}", codigoCliente);
                    return File(result, "application/pdf", $"reporte_movimientos{codigoCliente}.pdf");
                }
                else
                {
                    _logger.LogInformation("No se ha encontrado el movimiento {@id}", codigoCliente);
                    return StatusCode(StatusCodes.Status204NoContent, "No se ha encontrado el movimiento.");
                }
            }
            catch (ReglaNegociosException ex)
            {
                _logger.LogError(ex, "Ha ocurrido un error  al mostrar el movimiento {@id}", codigoCliente);
                return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
                {
                    ErrorType = ErrorType.VALIDACION_PARAMETROS_ENTRADA,
                    ErrorDescription = ex.Message,
                });
            }
            catch (System.Exception e)
            {
                _logger.LogError(e, "Respuesta del servidor obre la obtención de la información del movimiento {@id}", codigoCliente);
                return StatusCode(500, new ErrorResponse
                {
                    ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                    ErrorDescription = "Ha ocurrido un error  al mostrar el movimientos. Intente nuevamente mas tarde",
                });
            }
        }
    }
}