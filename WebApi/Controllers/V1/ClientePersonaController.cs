using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSwag.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Enums;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Contracts.Responses;
using WebApi.Core.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers.V1;

[ApiVersion("1.0")]
[Route("v{version:apiVersion}/api")]
[ApiController]
[OpenApiTag("ClientePersonaController", Description = "Controlador de Endpoints para Cliente y Persona.")]
public class ClientePersonaController : BaseApiController
{
    private readonly ILogger<ClientePersonaController> _logger;
    private readonly IClientePersonaService _clientePersonaService;

    public ClientePersonaController(ILogger<ClientePersonaController> logger,
        IClientePersonaService clientePersonaService)
    {
        _logger = logger;
        _clientePersonaService = clientePersonaService;
    }

    [HttpPost("persona")]
    [OpenApiOperation("InsertarPersona", description: "Inserta los datos de la persona en la tabla Persona.")]
    [SwaggerResponse(StatusCodes.Status201Created, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve una lista de las personas que fueron insertadas en la base de datos.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> InsertarPersona([Description("Datos de la persona a insertar en tabla Persona")][FromBody] PersonaRequest persona)
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
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpPost("cliente")]
    [OpenApiOperation("InsertarCliente", description: "Crea un usuario para una persona.")]
    [SwaggerResponse(StatusCodes.Status201Created, typeof(ClienteResponse), Description = "Operación exitosa. Devuelve los datos insertados para creación de cuenta.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> InsertarCliente([Description("Datos a insertar para creación de cuenta")][FromBody] ClienteRequest parametros)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de InsertarCliente {@parametros}", parametros);
            var result = await _clientePersonaService.InsertarCliente(parametros);

            if (result.PersonaId != 0)
            {
                _logger.LogInformation("Fin de solicitud de InsertarCliente. Se ha creado el usuario con éxito  {@parametros}", parametros);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            else
            {
                _logger.LogInformation("La cuenta no se encuentra excepcionada {@parametros}", parametros);
                return StatusCode(StatusCodes.Status400BadRequest, "Ingrese un numero de cuenta válido.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al crear la cuenta {@parametros}", parametros);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos para creación de cuenta {@parametros}", parametros);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al crear su cuenta. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpGet("persona/{id}")]
    [OpenApiOperation("ObtenerPersona", description: "Obtiene los datos de la persona en base a su id.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve una lista de las personas que fueron insertadas en la base de datos.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(PersonaResponse), Description = "No se ha encontrado coicidencias.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ObtenerPersona([Description("Id de la persona")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ObtenerPersona {@id}", id);
            var result = await _clientePersonaService.ObtenerPersona(new CodigoPersonaRequest() { PersonaId = id });

            if (result.IdPersona != 0)
            {
                _logger.LogInformation("Se muestran los datos de la persona {@id}", id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al obtener los datos de la persona {@id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la obtención de los datos de las personas {@id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al obtener los datos de la persona. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpGet("cliente/{id}")]
    [OpenApiOperation("ObtenerCliente", description: "Obtiene los datos de la cuenta de la persona en base a su id persona.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(ClienteResponse), Description = "Operación exitosa. Devuelve la cuenta de la persona.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(ClienteResponse), Description = "No se ha encontrado coicidencias.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ObtenerCliente([Description("Id de la persona")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ObtenerCliente {@id}", id);
            var result = await _clientePersonaService.ObtenerCliente(id);

            if (result.PersonaId != 0)
            {
                _logger.LogInformation("Se muestran los datos de la persona {@id}", id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al insertar los datos de las personas {@id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de las personas {@id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
            });
        }
    }
    [HttpPut("persona/{id}")]
    [OpenApiOperation("ActualizarPersona", description: "Actualiza los datos de una persona")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaUpdateDTO), Description = "Operación exitosa. Devuelve una los datos actualizados de la persona.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(PersonaUpdateDTO), Description = "No se ha encontrado datos para actualizar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ActualizarPersona([Description("Id de la persona a actualizar en tabla persona")][FromRoute] int id, [FromBody] PersonaRequest persona)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ActualizarPersona {@id}", id);
            var result = await _clientePersonaService.ActualizarPersona(persona, new CodigoPersonaRequest() { PersonaId = id });

            if (result.Nombre != null)
            {
                _logger.LogInformation("Se muestran los datos de la persona {@id}", id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al actualizar los datos de la persona {@id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la actualización de los datos de la persona {@id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al actualizar los datos de las personas. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpPut("cliente/{id}")]
    [OpenApiOperation("ActualizarCliente", description: "Actualiza los datos del cliente.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(ClienteUpdateDTO), Description = "Operación exitosa. Devuelve los datos actualizados del cliente.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(ClienteUpdateDTO), Description = "No se ha encontrado datos para actualizar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ActualizarCliente([Description("Id de la persona que desea actualizar sus datos")][FromRoute] int id, [Description("Contraseña Anterior del cliente, necesario para actualizar datos del cliente")][FromHeader] string passwordAnterior, [FromBody] ClienteUpdateRequest cliente)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ActualizarPersona {@cliente}", cliente);
            if (string.IsNullOrEmpty(passwordAnterior))
            {
                _logger.LogInformation("El header 'passwordAnterior' es obligatorio. {@passwordAnterior}", passwordAnterior);
                return StatusCode(StatusCodes.Status400BadRequest, "El header 'passwordAnterior' es obligatorio.");
            }
            var result = await _clientePersonaService.ActualizarCliente(id, cliente, passwordAnterior);

            if (result.Estado != null)
            {
                _logger.LogInformation("Se muestran los datos del cliente {@cliente}", cliente);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@cliente}", cliente);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al actualizar los datos del cliente {@cliente}", cliente);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la actualización de los datos del cliente {@cliente}", cliente);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al actualizar los datos del cliente. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpDelete("persona/{id}")]
    [OpenApiOperation("EliminarPersona", description: "Elimina registro de la tabla Persona en base al id persona.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(ClienteResponse), Description = "Operación exitosa. Devuelve los datos de la persona eliminada de la base de datos.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(ClienteResponse), Description = "No se han encontrado coincidencias para eliminar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> EliminarPersona([Description("Id del cliente")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de EliminarPersona {@id}", id);
            var result = await _clientePersonaService.EliminarPersona(id);

            if (result)
            {
                _logger.LogInformation("Se muestran los datos de la persona {@id}", id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No existen datos a eliminar {@id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al insertar los datos de las personas {@id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de las personas {@id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpDelete("cliente/{id}")]
    [OpenApiOperation("EliminarCliente", description: "Elimina registro de la tabla Cliente en base al id persona.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(ClienteResponse), Description = "Operación exitosa. Devuelve los datos del cliente eliminado en la base de datos.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(ClienteResponse), Description = "No se han encontrado coincidencias para eliminar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> EliminarCliente([Description("Id de la persona")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de EliminarCliente {@id}", id);
            var result = await _clientePersonaService.EliminarCliente(id);

            if (result)
            {
                _logger.LogInformation("Se muestran los datos de la persona {@id}", id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se existen datos a eliminar {@id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se existen datos a eliminar");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al insertar los datos de las personas {@id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ErrorType.validacion_parametro_entrada,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de las personas {@id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al insertar los datos de las personas. Intente nuevamente mas tarde",
            });
        }
    }
    //[HttpPost("clientes")]
    //[OpenApiOperation("InsertarClienteNoVisible", description: "Oculta las cuentas de los clientes proporcionados, insertándolos en la tabla ver_marcas_cuenta.")]
    //[EndpointDescription("Oculta las cuentas de los clientes proporcionados, insertándolos en la tabla ver_marcas_cuenta.")]
    //[SwaggerResponse(StatusCodes.Status201Created, typeof(List<PersonaEntity>), Description = "Operación exitosa. Retorna una lista de firmantes insertados en la tabla ver_marcas_cuenta.")]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    //[SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(string), Description = "No autorizado para realizar la operación.")]
    //[SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    //public async Task<IActionResult> InsertarClienteNoVisible([Description("Array de clientes a ocultar")][FromBody] MarcasCuentaRequest cuenta)
    //{
    //    _logger.LogInformation("Inicio solicitud de InsertarClienteNoVisible {@cuenta}", cuenta);

    //    try
    //    {
    //        var result = await _configuracionesCuentaService.InsertarClienteNoVisible(cuenta);
    //        if (result.Any())
    //        {
    //            return StatusCode(StatusCodes.Status201Created, result);
    //        }
    //        else
    //        {
    //            _logger.LogError("Todos los datos ya fueron registrados con anterioridad {@cuenta}", cuenta);
    //            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
    //            {
    //                ErrorType = ErrorType.datos_duplicados,
    //                ErrorDescription = "Todos los datos ya fueron registrados con anterioridad ",
    //            });
    //        }
    //    }
    //    catch (ReglaNegociosException ex)
    //    {
    //        _logger.LogError(ex, "Ha ocurrido un error en las validaciones del negocio al ocultar la cuenta del cliente {@cuenta}", cuenta);
    //        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
    //        {
    //            ErrorType = ErrorType.validacion_parametro_entrada,
    //            ErrorDescription = ex.Message,
    //        });
    //    }
    //    catch (System.Exception e)
    //    {
    //        _logger.LogError(e, "Respuesta del servidor sobre la inserción de datos {@cuenta}", cuenta);
    //        return StatusCode(500, new ErrorResponse
    //        {
    //            ErrorType = ErrorType.error_interno_servidor,
    //            ErrorDescription = "Ha ocurrido un error en las validaciones del negocio al ocultar la cuenta del cliente. Intente nuevamente mas tarde",
    //        });
    //    }
    //}

    //[HttpDelete("movimientos")]
    //[OpenApiOperation("EliminarClienteNoVisible", description: "Desoculta las cuentas de los clientes proporcionados, eliminándolos de la tabla ver_marcas_cuenta.")]
    //[SwaggerResponse(StatusCodes.Status200OK, typeof(List<PersonaEntity>), Description = "Operación exitosa. Retorna una lista de firmantes eliminados de la tabla ver_marcas_cuenta.")]
    //[SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    //[SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(string), Description = "No autorizado para realizar la operación.")]
    //[SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    //public async Task<IActionResult> EliminarClienteNoVisible([Description("Array de clientes a visibilizar")][FromBody] MarcasCuentaRequest cuenta)
    //{
    //    _logger.LogInformation("Inicio solicitud de EliminarClienteNoVisible {@cuenta}", cuenta);

    //    try
    //    {
    //        var result = await _configuracionesCuentaService.EliminarClienteNoVisible(cuenta);
    //        if (result.Any())
    //        {
    //            return StatusCode(StatusCodes.Status200OK, result);
    //        }
    //        else
    //        {
    //            _logger.LogError("Todos los datos ya fueron registrados con anterioridad {@cuenta}", cuenta);
    //            return StatusCode(StatusCodes.Status204NoContent, new ErrorResponse()
    //            {
    //                ErrorType = ErrorType.datos_no_encontrados,
    //                ErrorDescription = "No se encontraron registros para eliminar",
    //            });
    //        }
    //    }
    //    catch (ReglaNegociosException ex)
    //    {
    //        _logger.LogError(ex, "Ha ocurrido un error en las validaciones del negocio al desocultar la cuenta del cliente {@cuenta}", cuenta);
    //        return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
    //        {
    //            ErrorType = ErrorType.validacion_parametro_entrada,
    //            ErrorDescription = ex.Message,
    //        });
    //    }
    //    catch (System.Exception e)
    //    {
    //        _logger.LogError(e, "Respuesta del servidor sobre la eliminacion de datos {@cuenta}", cuenta);
    //        return StatusCode(500, new ErrorResponse
    //        {
    //            ErrorType = ErrorType.error_interno_servidor,
    //            ErrorDescription = "Ha ocurrido un error en las validaciones del negocio al desocultar la cuenta del cliente. Intente nuevamente mas tarde",
    //        });
    //    }
    //}
}