using Core.Contracts.Enums;
using FluentValidation;
using System;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class MovimientosRequestValidator : AbstractValidator<MovimientosRequest>
    {
        public MovimientosRequestValidator()
        {
            RuleFor(x => x.Fecha)
               .NotEmpty().WithMessage("La fecha es obligatoria.")
               .Matches(@"^\d{2}/\d{2}/\d{4}$").WithMessage("El formato de fecha debe ser dd/mm/yyyy.");


            RuleFor(x => x.TipoMovimiento)
            .NotEmpty().WithMessage("El tipo de movimiento es obligatorio.")
            .Must(x => Enum.IsDefined(typeof(TipoMovimiento), x))
            .WithMessage("El tipo de movimiento no es válido. 0 - Deposito | 1 - Retiro")
            .When(x => x.TipoMovimiento != TipoMovimiento.DEPOSITO && x.TipoMovimiento != TipoMovimiento.RETIRO)
            .WithMessage("El tipo de movimiento no es válido. 0 - Deposito | 1 - Retiro");

            RuleFor(x => x.Valor)
                .NotEmpty().WithMessage("El valor del movimiento es obligatorio.")
                .NotEqual(0).WithMessage("El valor del movimiento no puede ser cero.");
            
        }
    }
}