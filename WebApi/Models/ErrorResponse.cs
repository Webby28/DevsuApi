using WebApi.Core.Contracts.Enums;

namespace WebApi.WebApi.Models
{
    /// <summary>
    /// Clase predeterminada que retornara el error y el mensaje de error en caso de producirse excepciones
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// Tipo de Error
        /// </summary>
        public ErrorType ErrorType { get; set; }

        /// <summary>
        /// Mensaje de Error
        /// </summary>
        public string ErrorDescription { get; set; }
    }
}