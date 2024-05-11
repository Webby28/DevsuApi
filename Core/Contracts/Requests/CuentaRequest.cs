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
        public required string TipoCuenta { get; set; }

        /// <summary>
        /// Saldo inicial de la cuenta.
        /// </summary>
        [Description("Saldo inicial de la cuenta")]
        public int SaldoInicial { get; set; }

        /// <summary>
        /// Estado de la cuenta.
        /// </summary>
        [Description("Estado de la cuenta")]
        public required string Estado { get; set; }

        /// <summary>
        /// Cliente asociado a la cuenta.
        /// </summary>
        [Description("Cliente asociado a la cuenta")]
        public int IdCliente { get; set; }
    }
}