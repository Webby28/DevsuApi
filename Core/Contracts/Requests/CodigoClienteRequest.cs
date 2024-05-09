namespace WebApi.Core.Contracts.Requests
{
    /// <summary>
    /// Clase para obtener los datos del cliente en base a id persona
    /// </summary>
    public class CodigoClienteRequest
    {
        /// <summary>
        /// Identificador de la persona asociada al cliente.
        /// </summary>
        public int PersonaId { get; set; }
    }
}