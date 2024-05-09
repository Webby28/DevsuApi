using FluentValidation;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class ClienteUpdateRequestValidator : AbstractValidator<ClienteUpdateRequest>
    {
        public ClienteUpdateRequestValidator()
        {
            RuleFor(x => x.Contraseña)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");

            RuleFor(x => x.Estado)
                .NotEmpty().WithMessage("El estado es obligatorio.");
        }
    }
}