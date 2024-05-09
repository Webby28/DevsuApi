using System.ComponentModel.DataAnnotations;

namespace Core.Contracts.Enums
{
    public enum TipoMovimiento
    {
        [Display(Name = "Depósito")]
        DEPOSITO = 0,

        [Display(Name = "Retiro")]
        RETIRO = 1
    }
}