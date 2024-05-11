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
    [OpenApiOperation("InsertarPersona", description: "Endpoint que inserta datos en la tabla persona")]
    [SwaggerResponse(StatusCodes.Status201Created, typeof(PersonaEntity), Description = "Operación exitosa. Devuelve los datos de la persona que fue registrada en la base de datos.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> InsertarPersona([Description("Datos que se insertarán en la tabla persona")][FromBody] PersonaRequest persona)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de InsertarPersona {@Persona}", persona);
            var result = await _clientePersonaService.InsertarPersona(persona);

            if (!string.IsNullOrWhiteSpace(result.Nombre))
            {
                _logger.LogInformation("Los datos de la persona se ha insertado con éxito {@Persona}", persona);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            else
            {
                _logger.LogInformation("Ha ocurrido un error  al insertar los datos de la persona {@Persona}", persona);
                return StatusCode(StatusCodes.Status400BadRequest, "Ha ocurrido un error  al insertar los datos de las personas.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al insertar los datos de la persona {@Persona}", persona);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos de la persona {@Persona}", persona);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al insertar los datos de la persona. Intente nuevamente mas tarde.",
            });
        }
    }

    [HttpPost("cliente")]
    [OpenApiOperation("InsertarCliente", description: "Endpoint que crea un usuario para una persona.")]
    [SwaggerResponse(StatusCodes.Status201Created, typeof(ClienteEntity), Description = "Operación exitosa. Devuelve los datos del cliente que fue registrado en la base de datos.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> InsertarCliente([Description("Datos que se insertarán en tabla cliente")][FromBody] ClienteRequest parametros)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de InsertarCliente {@Parametros}", parametros);
            var result = await _clientePersonaService.InsertarCliente(parametros);

            if (result.PersonaId != 0)
            {
                _logger.LogInformation("Fin de solicitud de InsertarCliente. Se ha creado el usuario con éxito  {@Parametros}", parametros);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            else
            {
                _logger.LogInformation("Ha ocurrido un error  al crear el usuario. {@Parametros}", parametros);
                return StatusCode(StatusCodes.Status400BadRequest, "Hubo un error error al crear el usuario.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al crear el usuario {@Parametros}", parametros);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la inserción de los datos para creación de usuario {@Parametros}", parametros);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al crear su usuario. Intente nuevamente mas tarde.",
            });
        }
    }

    [HttpGet("persona/{id}")]
    [OpenApiOperation("ObtenerPersona", description: "Endpoint que obtiene los datos de la persona en base a su id.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaResponse), Description = "Operación exitosa. Devuelve una los datos de la persona.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(PersonaResponse), Description = "No se ha encontrado coicidencias.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ObtenerPersona([Description("Id de la persona")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ObtenerPersona {@Id}", id);
            var result = await _clientePersonaService.ObtenerPersona(id);

            if (result.IdPersona != 0)
            {
                _logger.LogInformation("Se muestran los datos de la persona {@Id}", id);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@Id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al obtener los datos de la persona {@Id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la obtención de los datos de la persona {@Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al obtener los datos de la persona. Intente nuevamente mas tarde.",
            });
        }
    }

    [HttpGet("cliente/{id}")]
    [OpenApiOperation("ObtenerCliente", description: "Endpoint que obtiene los datos del cliente.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(ClienteEntity), Description = "Operación exitosa. Devuelve el cliente.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(ClienteEntity), Description = "No se ha encontrado coicidencias.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ObtenerCliente([Description("Id persona asociada al cliente")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ObtenerCliente {@Id}", id);
            var result = await _clientePersonaService.ObtenerCliente(id);

            if (result.PersonaId != 0)
            {
                _logger.LogInformation("Se muestran los datos del cliente {@Result}", result);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@Id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al obtener los datos del cliente {@Id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la obtención de los datos del cliente {@Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al obtener los datos del cliente. Intente nuevamente mas tarde.",
            });
        }
    }

    [HttpPut("persona/{id}")]
    [OpenApiOperation("ActualizarPersona", description: "Endpoint que actualiza los datos de una persona")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(PersonaUpdateDto), Description = "Operación exitosa. Devuelve un modelo con los datos nuevos de la persona.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(PersonaUpdateDto), Description = "No se ha encontrado datos para actualizar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ActualizarPersona([Description("Id persona a actualizar")][FromRoute] int id, [FromBody] PersonaRequest persona)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ActualizarPersona {@Id} {@Persona}", id, persona);
            var result = await _clientePersonaService.ActualizarPersona(persona, id);

            if (result.Nombre != null)
            {
                _logger.LogInformation("Se muestran los datos nuevos de la persona {@Result}", result);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@Result}", result);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al actualizar los datos de la persona {@Id} {@Persona}", id, persona);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la actualización de los datos de la persona {@Id} {@Persona}", id, persona);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al actualizar los datos de la persona. Intente nuevamente mas tarde.",
            });
        }
    }

    [HttpPut("cliente/{id}")]
    [OpenApiOperation("ActualizarCliente", description: "Endpoint que actualiza los datos de un cliente.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(ClienteUpdateDto), Description = "Operación exitosa. Devuelve un modelo con los datos nuevos del cliente.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(ClienteUpdateDto), Description = "No se ha encontrado datos para actualizar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> ActualizarCliente([Description("Id persona asociada al cliente")][FromRoute] int id, [Description("Contraseña Anterior del cliente, necesario para actualizar datos del cliente")][FromHeader] string passwordAnterior, [Description("Datos del cliente a actualizar")][FromBody] ClienteUpdateRequest cliente)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de ActualizarCliente {@Cliente}", cliente);
            if (string.IsNullOrEmpty(passwordAnterior))
            {
                _logger.LogInformation("El header 'passwordAnterior' es obligatorio. {@PasswordAnterior}", passwordAnterior);
                return StatusCode(StatusCodes.Status400BadRequest, "El header 'passwordAnterior' es obligatorio.");
            }
            var result = await _clientePersonaService.ActualizarCliente(id, cliente, passwordAnterior);

            if (result != null)
            {
                _logger.LogInformation("Se muestran los datos actualizados del cliente {@Cliente}", cliente);
                return StatusCode(StatusCodes.Status200OK, result);
            }
            else
            {
                _logger.LogInformation("No se han encontrado coincidencias {@Cliente}", cliente);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al actualizar los datos del cliente {@Cliente}", cliente);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la actualización de los datos del cliente {@Cliente}", cliente);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al actualizar los datos del cliente. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpDelete("persona/{id}")]
    [OpenApiOperation("EliminarPersona", description: "Endpoint que elimina registro de la tabla persona.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(void), Description = "Operación exitosa. Devuelve true si se ha eliminado.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se han encontrado coincidencias para eliminar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> EliminarPersona([Description("Id de la persona a eliminar")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de EliminarPersona {@Id}", id);
            var result = await _clientePersonaService.EliminarPersona(id);

            if (result)
            {
                _logger.LogInformation("Se ha eliminado el registro de la persona {@Id}", id);
                return StatusCode(StatusCodes.Status200OK, "Se ha eliminado el registro con éxito.");
            }
            else
            {
                _logger.LogInformation("No existen datos a eliminar {@Id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se han encontrado coincidencias.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al eliminar el registro de la persona {@Id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la eliminacion de los datos  {@Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al eliminar el registro. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpDelete("cliente/{id}")]
    [OpenApiOperation("EliminarCliente", description: "Endpoint que elimina registro de la tabla cliente.")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(void), Description = "Operación exitosa. Devuelve true si se ha eliminado.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se han encontrado coincidencias para eliminar.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<IActionResult> EliminarCliente([Description("Id de la persona asociada al cliente")][FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicio solicitud de EliminarCliente {@Id}", id);
            var result = await _clientePersonaService.EliminarCliente(id);

            if (result)
            {
                _logger.LogInformation("Se ha eliminado el registro del cliente  {@Id}", id);
                return StatusCode(StatusCodes.Status200OK, "Se ha eliminado el cliente.");
            }
            else
            {
                _logger.LogInformation("No se existen datos a eliminar {@Id}", id);
                return StatusCode(StatusCodes.Status204NoContent, "No se existen datos a eliminar");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al eliminar los datos {@Id}", id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Respuesta del servidor sobre la eliminación de los datos {@Id}", id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ha ocurrido un error  al eliminar el registro. Intente nuevamente mas tarde",
            });
        }
    }

    [HttpPatch("persona/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(int), Description = "Operación exitosa. Se actualizó el estado de la cuenta.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se ha encontrado la cuenta.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<ActionResult> ActualizarPersonaPatch([FromRoute] int id, [FromBody] ModificarEstadoRequest requestModifica)
    {
        try
        {
            _logger.LogInformation("Iniciando el proceso de modificación del estado. {@RequestModifica}, {@Id}", requestModifica, id);
            var result = await _clientePersonaService.ActualizarEstado(requestModifica.Estado, id, Tabla.PERSONA);
            if (result != 204)
            {
                _logger.LogInformation("Se actualizó el estado de la persona con éxito {@Id}, {@RequestModifica}", id, requestModifica);
                return StatusCode(StatusCodes.Status200OK, "Se actualizó el estado de la persona con éxito");
            }
            else
            {
                _logger.LogInformation("No se ha encontrado la persona {@Id}, {@RequestModifica}", id, requestModifica);
                return StatusCode(StatusCodes.Status204NoContent, "No se ha encontrado la persona.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al actualizar el estado {@RequestModifica}, {@Id}", requestModifica, id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Ocurrio un error al momento de gestionar el estado {@RequestModifica}, cliente {@Id}", requestModifica, id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ocurrio un error al momento de modificar el estado."
            });
        }
    }

    [HttpPatch("cliente/{id}")]
    [SwaggerResponse(StatusCodes.Status200OK, typeof(int), Description = "Operación exitosa. Se actualizó el estado de la cuenta.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, typeof(void), Description = "No se ha encontrado la cuenta.")]
    [SwaggerResponse(StatusCodes.Status400BadRequest, typeof(ErrorResponse), Description = "La solicitud es incorrecta.")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, typeof(ErrorResponse), Description = "No autorizado para realizar la operación.")]
    [SwaggerResponse(StatusCodes.Status500InternalServerError, typeof(ErrorResponse), Description = "Error interno del servidor.")]
    public async Task<ActionResult> ActualizarClientePatch([FromRoute] int id, [FromBody] ModificarEstadoRequest requestModifica)
    {
        try
        {
            _logger.LogInformation("Iniciando el proceso de modificación del estado. {@RequestModifica}, {@Id}", requestModifica, id);
            var result = await _clientePersonaService.ActualizarEstado(requestModifica.Estado, id, Tabla.CLIENTE);
            if (result != 204)
            {
                _logger.LogInformation("Se actualizó el estado de la persona con éxito {@Id}, {@RequestModifica}", id, requestModifica);
                return StatusCode(StatusCodes.Status200OK, "Se actualizó el estado de la persona con éxito");
            }
            else
            {
                _logger.LogInformation("No se ha encontrado la persona {@Id}, {@RequestModifica}", id, requestModifica);
                return StatusCode(StatusCodes.Status204NoContent, "No se ha encontrado la persona.");
            }
        }
        catch (ReglaNegociosException ex)
        {
            _logger.LogError(ex, "Ha ocurrido un error  al actualizar el estado {@RequestModifica}, {@Id}", requestModifica, id);
            return StatusCode(StatusCodes.Status400BadRequest, new ErrorResponse()
            {
                ErrorType = ex.CodigoError,
                ErrorDescription = ex.Message,
            });
        }
        catch (System.Exception e)
        {
            _logger.LogError(e, "Ocurrio un error al momento de gestionar el estado {@RequestModifica}, cliente {@Id}", requestModifica, id);
            return StatusCode(500, new ErrorResponse
            {
                ErrorType = ErrorType.ERROR_INTERNO_EN_SERVIDOR,
                ErrorDescription = "Ocurrio un error al momento de modificar el estado."
            });
        }
    }
}