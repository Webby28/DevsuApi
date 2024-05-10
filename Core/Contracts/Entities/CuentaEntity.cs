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
    public int SaldoInicial { get; set; }

    [Required]
    [Description("Estado de la cuenta. A (Activo) I (Inactivo)")]
    public char Estado { get; set; } = 'A';

    [Required]
    [Description("Id del cliente asociado a la cuenta")]
    public int IdCliente { get; set; }
}