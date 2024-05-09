using Core.Contracts.Enums;
using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Core.Contracts.Requests
{
    /// <summary>
    /// Entidad para la tabla Movimientos.
    /// </summary>
    public class MovimientosRequest
    {
        /// <summary>
        /// Fecha del movimiento en formato de cadena "dd/mm/yyyy".
        /// </summary>
        [JsonProperty("fecha")]
        [Description("Fecha del movimiento")]
        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public string Fecha { get; set; }

        /// <summary>
        /// Tipo de movimiento.
        /// </summary>
        [Description("Tipo de movimiento")]
        public TipoMovimiento TipoMovimiento { get; set; }

        /// <summary>
        /// Valor del movimiento.
        /// </summary>
        [Description("Valor del movimiento")]
        public int Valor { get; set; }

        [Description("Numero de Cuenta asociada al movimiento")]
        public int NumeroCuenta { get; set; }
    }
}