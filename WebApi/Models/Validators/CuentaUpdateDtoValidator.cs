using FluentValidation;
using System.Collections.Generic;
using WebApi.Core.Contracts.Entities;

namespace WebApi.Models.Validators
{
    public class CuentaUpdateDtoValidator : AbstractValidator<CuentaUpdateDto>
    {
        public CuentaUpdateDtoValidator()
        {
            List<string> tiposCuentaValidos = new List<string> { "CC", "AH", "CH", "CAP" };

            RuleFor(x => x.TipoCuenta)
                .NotEmpty().WithMessage("El tipo de cuenta es obligatorio.")
                .Must(tipoCuenta => tiposCuentaValidos.Contains(tipoCuenta)).WithMessage("El campo Tipo Cuenta debe ser uno de los tipos válidos: 'CC' (Cuenta Corriente), 'AH' (Ahorro), 'CH' (Cuenta de Cheques), 'CAP' (Cuenta Ahorro Programado).");

            RuleFor(p => p.Estado)
                .NotNull().WithMessage("El campo Estado no debe ser nulo, favor verifique.")
                .Must(estado => estado.ToString().Length == 1).WithMessage("El campo Estado debe tener exactamente un carácter.")
                .Must(estado => estado == "A" || estado == "I").WithMessage("El campo Estado debe ser 'A' (Activo) o 'I' (Inactivo).");
        }
    }
}