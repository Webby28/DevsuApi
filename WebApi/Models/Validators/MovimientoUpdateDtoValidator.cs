using FluentValidation;
using WebApi.Core.Contracts.Entities;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class MovimientoUpdateDtoValidator : AbstractValidator<MovimientoUpdateDto>
    {
        public MovimientoUpdateDtoValidator()
        {
            RuleFor(x => x.TipoMovimiento)
            .NotEmpty().WithMessage("El tipo de movimiento es obligatorio.")
            .Must(tipo => tipo.Length == 1).WithMessage("El campo TipoMovimiento debe tener exactamente un carácter.")
            .Must(x => x == "0" || x == "1")
            .WithMessage("El tipo de movimiento no es válido. 0 - Depósito | 1 - Retiro");

            RuleFor(x => x.Valor)
              .InclusiveBetween(0, 9999999)
              .WithMessage("El valor del movimiento debe estar en el rango de 0 a 9999999.");
        }
    }
}