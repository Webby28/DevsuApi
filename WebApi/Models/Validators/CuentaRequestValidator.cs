using FluentValidation;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class CuentaRequestValidator : AbstractValidator<CuentaRequest>
    {
        public CuentaRequestValidator()
        {
            RuleFor(x => x.TipoCuenta)
                .NotEmpty().WithMessage("El tipo de cuenta es obligatorio.");

            RuleFor(x => x.SaldoInicial)
                .NotEmpty().WithMessage("El saldo inicial es obligatorio.")
                .GreaterThan(0).WithMessage("El saldo inicial debe ser mayor que cero.");

            RuleFor(p => p.Estado)
                .NotNull().WithMessage("El campo Estado no debe ser nulo, favor verifique.")
                .Must(estado => estado == 'A' || estado == 'I').WithMessage("El campo Estado debe ser 'A' (Activo) o 'I' (Inactivo).");
        }
    }
}