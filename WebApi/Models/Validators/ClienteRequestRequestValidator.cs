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

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("El estado es obligatorio.");
        }
    }
}