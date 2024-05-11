using System;

namespace Core.Contracts.Models
{
    public class MovimientoReporte
    {
        public int IdMovimiento { get; set; }
        public string Fecha { get; set; }
        public int Valor { get; set; }
        public int Saldo { get; set; }
        public int NumeroCuenta { get; set; }
    }

    public class MovimientosReporte
    {
        public List<MovimientoReporte> MovimientoReportes { get; set; } = new List<MovimientoReporte>();
    }
}
