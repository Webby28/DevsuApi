namespace WebApi.Core.Contracts.Responses
{
    /// <summary>
    /// Clase para consultar existencia de cuentas en ver_marcas_cuentas
    /// </summary>
    public class MovimientosResponse
    {
        public int IdMovimiento { get; set; }
        public int Saldo { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime FechaHasta { get; set; }
    }
}