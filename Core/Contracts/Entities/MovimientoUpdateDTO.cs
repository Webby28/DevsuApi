namespace WebApi.Core.Contracts.Entities;

public class MovimientoUpdateDto
{
    /// <summary>
    /// TipoMovimiento de movimiento
    /// </summary>
    public string TipoMovimiento { get; set; }

    /// <summary>
    /// Nuevo valor a actualizar
    /// </summary>
    public int Valor { get; set; } = 0;
}