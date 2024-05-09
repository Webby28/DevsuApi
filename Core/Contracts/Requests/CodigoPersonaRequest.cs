using System.ComponentModel;

namespace WebApi.Core.Contracts.Requests
{
    /// <summary>
    /// Clase para obtener los datos de la persona en base a su id
    /// </summary>
    public class CodigoPersonaRequest
    {
        /// <summary>
        /// Identificador de la persona.
        /// </summary>
        public int PersonaId { get; set; }       
    }
}