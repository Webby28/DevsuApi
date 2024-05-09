using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Core.Contracts.Entities;

public class ClienteUpdateDTO
{
    /// <summary>
    /// Id del cliente
    /// </summary>
    public int IdCliente { get; set; }
    /// <summary>
    /// Id de la Persona
    /// </summary>
    public int PersonaId { get; set; }
    /// <summary>
    /// Contraseña Nueva
    /// </summary>
    public required string Contraseña { get; set; }
    /// <summary>
    /// Estado del cliente
    /// </summary>
    public required string Estado { get; set; }
}