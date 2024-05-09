using System.ComponentModel;

namespace WebApi.Core.Contracts.Requests
{
    /// <summary>
    /// Clase para actualizar datos del cliente
    /// </summary>
    public class ClienteUpdateRequest
    {
        /// <summary>
        /// Contraseña nueva del cliente.
        /// </summary>
        [Description("Contraseña nueva del cliente")]
        public string Contraseña { get; set; }

        /// <summary>
        /// Estado del cliente.
        /// </summary>
        [Description("Estado del cliente")]
        public string Estado { get; set; }
    }
}