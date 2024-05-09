using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Core.Contracts.Entities;

public class CuentaEntity
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NumeroCuenta { get; set; }

    [Required]
    [Description("Tipo de cuenta")]
    public string TipoCuenta { get; set; }

    [Required]
    [Description("Saldo inicial de la cuenta")]
    public decimal SaldoInicial { get; set; }

    [Required]
    [Description("Estado de la cuenta")]
    public string Estado { get; set; }
}