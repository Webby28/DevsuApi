namespace WebApi.Core.Contracts.Entities;

public class MovimientoUpdateDTO
{


    /// <summary>
    /// TipoMovimiento de movimiento
    /// </summary>
    public char TipoMovimiento { get; set; }

    /// <summary>
    /// Nuevo valor a actualizar
    /// </summary>
    public int Valor { get; set; }
  
}