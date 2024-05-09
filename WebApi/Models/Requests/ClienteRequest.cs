using System.ComponentModel;

namespace WebApi.WebApi.Models.Requests
{
    /// <summary>
    /// Entidad para la tabla Cliente.
    /// </summary>
    public class ClienteRequest
    {
        /// <summary>
        /// Identificador único del cliente.
        /// </summary>
        public int IdCliente { get; set; }

        /// <summary>
        /// Identificador de la persona asociada al cliente.
        /// </summary>
        public int PersonaId { get; set; }

        /// <summary>
        /// Contraseña del cliente.
        /// </summary>
        [Description("Contraseña del cliente")]
        public string Contraseña { get; set; }

        /// <summary>
        /// Estado del cliente.
        /// </summary>
        [Description("Estado del cliente")]
        public string Estado { get; set; }
    }
}