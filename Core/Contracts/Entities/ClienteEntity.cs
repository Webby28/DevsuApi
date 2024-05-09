using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Core.Contracts.Entities;
public class ClienteEntity
{
    [Required]
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdCliente { get; set; }

    [Required]
    public int PersonaId { get; set; }

    [Required]
    [Description("Contraseña del cliente")]
    public required string Contraseña { get; set; }

    [Required]
    [Description("Estado del cliente")]
    public required string Estado { get; set; }
}