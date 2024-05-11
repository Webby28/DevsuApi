

namespace WebApi.Core.Contracts.Requests
{
    public class ModificarEstadoRequest
    {
        /// <summary>
        /// Identificador que corresponde al estado que se actualizará en tablas
        /// </summary>
        public required string Estado { get; set; }
    }
  
}