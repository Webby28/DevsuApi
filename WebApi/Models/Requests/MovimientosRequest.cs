using System.ComponentModel;

namespace WebApi.WebApi.Models.Requests
{
    /// <summary>
    /// Entidad para la tabla Movimientos.
    /// </summary>
    public class MovimientosRequest
    {
        /// <summary>
        /// Identificador único del movimiento.
        /// </summary>
        public int IdMovimiento { get; set; }

        /// <summary>
        /// Fecha del movimiento.
        /// </summary>
        [Description("Fecha del movimiento")]
        public DateTime Fecha { get; set; }

        /// <summary>
        /// Tipo de movimiento.
        /// </summary>
        [Description("Tipo de movimiento")]
        public string TipoMovimiento { get; set; }

        /// <summary>
        /// Valor del movimiento.
        /// </summary>
        [Description("Valor del movimiento")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Saldo resultante después del movimiento.
        /// </summary>
        [Description("Saldo resultante después del movimiento")]
        public decimal Saldo { get; set; }
    }
}