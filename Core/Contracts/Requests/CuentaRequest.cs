using System.ComponentModel;

namespace WebApi.Core.Contracts.Requests
{
    /// <summary>
    /// Entidad para la tabla Cuenta.
    /// </summary>
    public class CuentaRequest
    {
        /// <summary>
        /// Número de cuenta único.
        /// </summary>
        public int NumeroCuenta { get; set; }

        /// <summary>
        /// Tipo de cuenta.
        /// </summary>
        [Description("Tipo de cuenta")]
        public string TipoCuenta { get; set; }

        /// <summary>
        /// Saldo inicial de la cuenta.
        /// </summary>
        [Description("Saldo inicial de la cuenta")]
        public decimal SaldoInicial { get; set; }

        /// <summary>
        /// Estado de la cuenta.
        /// </summary>
        [Description("Estado de la cuenta")]
        public string Estado { get; set; }
    }
}