using FluentValidation;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class ClienteRequestRequestValidator : AbstractValidator<ClienteRequest>
    {
        public ClienteRequestRequestValidator()
        {
            RuleFor(x => x.Contraseña)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");

            RuleFor(p => p.Estado)
                .NotNull().WithMessage("El campo Estado no debe ser nulo, favor verifique.")
                .Must(estado => estado.ToString().Length == 1).WithMessage("El campo Estado debe tener exactamente un carácter.")
                .Must(estado => estado == "A" || estado == "I").WithMessage("El campo Estado debe ser 'A' (Activo) o 'I' (Inactivo).");
        }
    }
}