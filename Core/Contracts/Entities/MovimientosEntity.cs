using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Core.Contracts.Entities;

public class MovimientosEntity
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdMovimiento { get; set; }

    [Required]
    [Description("Fecha del movimiento")]
    public DateTime Fecha { get; set; }

    [Required]
    [Description("Tipo de movimiento")]
    public required string TipoMovimiento { get; set; }

    [Required]
    [Description("Valor del movimiento")]
    public decimal Valor { get; set; }

    [Required]
    [Description("Saldo resultante después del movimiento")]
    public decimal Saldo { get; set; }
    [Required]
    [Description("Numero de cuenta asociada al movimiento")]
    public int NumeroCuenta { get; set; }
}