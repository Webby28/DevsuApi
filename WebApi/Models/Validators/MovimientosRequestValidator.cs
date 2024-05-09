using FluentValidation;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class MovimientosRequestValidator : AbstractValidator<MovimientosRequest>
    {
        public MovimientosRequestValidator()
        {
            RuleFor(x => x.Fecha)
                .NotEmpty().WithMessage("La fecha es obligatoria.");

            RuleFor(x => x.TipoMovimiento)
                .NotEmpty().WithMessage("El tipo de movimiento es obligatorio.");

            RuleFor(x => x.Valor)
                .NotEmpty().WithMessage("El valor del movimiento es obligatorio.")
                .NotEqual(0).WithMessage("El valor del movimiento no puede ser cero.");

            RuleFor(x => x.Saldo)
                .NotEmpty().WithMessage("El saldo es obligatorio.");
        }
    }
}