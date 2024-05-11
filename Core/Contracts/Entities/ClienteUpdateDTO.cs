namespace WebApi.Core.Contracts.Entities;

public class ClienteUpdateDto
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
    public string Contraseña { get; set; }

    /// <summary>
    /// Estado del cliente
    /// </summary>
    public char Estado { get; set; }
}