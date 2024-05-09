using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
