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
    public char TipoMovimiento { get; set; }

    [Required]
    [Description("Valor del movimiento")]
    public int Valor { get; set; }

    [Required]
    [Description("Saldo resultante después del movimiento")]
    public int Saldo { get; set; }

    [Required]
    [Description("Numero de cuenta asociada al movimiento")]
    public int NumeroCuenta { get; set; }

    [Description("Fecha en que se registró la operación")]
    public DateTime FechaRegistro { get; set; }

    [Description("Estado del movimiento. P (Pendiente) C (Completado)")]
    public char Estado { get; set; } = 'P';
}