using System.ComponentModel;

namespace WebApi.WebApi.Models.Requests
{
    /// <summary>
    /// Entidad para la tabla Persona.
    /// </summary>
    public class PersonaRequest
    {
        ///// <summary>
        ///// Identificador único de la persona.
        ///// </summary>
        //public int IdPersona { get; set; }

        /// <summary>
        /// Nombre de la persona.
        /// </summary>
        [Description("Nombre de la persona")]
        public required string Nombre { get; set; }

        /// <summary>
        /// Género de la persona.
        /// </summary>
        [Description("Género de la persona")]
        public required string Genero { get; set; }

        /// <summary>
        /// Edad de la persona.
        /// </summary>
        [Description("Edad de la persona")]
        public int Edad { get; set; }

        /// <summary>
        /// Identificación de la persona.
        /// </summary>
        [Description("Identificación de la persona")]
        public required string Identificacion { get; set; }

        /// <summary>
        /// Dirección de la persona.
        /// </summary>
        [Description("Dirección de la persona")]
        public string? Direccion { get; set; }

        /// <summary>
        /// Teléfono de la persona.
        /// </summary>
        [Description("Teléfono de la persona")]
        public string? Telefono { get; set; }
    }
}