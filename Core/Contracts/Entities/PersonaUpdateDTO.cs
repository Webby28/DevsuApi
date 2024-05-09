using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Core.Contracts.Entities;

public class PersonaUpdateDTO
{
    public required string Nombre { get; set; }
    public required string Genero { get; set; }
    public int Edad { get; set; }
    public required string Identificacion { get; set; }
    public required string Direccion { get; set; }
    public required string Telefono { get; set; }
}