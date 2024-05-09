using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Core.Contracts.Entities;

public class PersonaEntity
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPersona { get; set; }

    [Required]
    [Description("Nombre de la persona")]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [Description("Género de la persona")]
    public string Genero { get; set; } = string.Empty;

    [Required]
    [Description("Edad de la persona")]
    public int Edad { get; set; }

    [Required]
    [Description("Identificación de la persona")]
    public string Identificacion { get; set; } = string.Empty;

    [Description("Dirección de la persona")]
    public string? Direccion { get; set; }

    [Description("Teléfono de la persona")]
    public string? Telefono { get; set; }
}