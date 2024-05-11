using System.ComponentModel;

namespace WebApi.Core.Contracts.Requests
{
    /// <summary>
    /// Clase para validar contraseña del cliente
    /// </summary>
    public class ClientePassword
    {
        /// <summary>
        /// Identificador de la persona asociada al cliente.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Contraseña anterior del cliente.
        /// </summary>
        [Description("Contraseña del cliente")]
        public string ContraseñaAnterior { get; set; }
    }
}