using FluentValidation;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    public class PersonaRequestValidator : AbstractValidator<PersonaRequest>
    {
        public PersonaRequestValidator()
        {
            RuleFor(x => x.Nombre)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.");

            RuleFor(x => x.Genero)
                .NotEmpty().WithMessage("El género es obligatorio.");

            RuleFor(x => x.Edad)
                .NotEmpty().WithMessage("La edad es obligatoria.")
                .GreaterThan(0).WithMessage("La edad debe ser mayor que cero.");

            RuleFor(x => x.Identificacion)
                .NotEmpty().WithMessage("La identificación es obligatoria.")
                .MaximumLength(20).WithMessage("La identificación no puede tener más de 20 caracteres.");

            RuleFor(x => x.Direccion)
                .NotEmpty().WithMessage("La dirección es obligatoria.")
                .MaximumLength(200).WithMessage("La dirección no puede tener más de 200 caracteres.");

            RuleFor(x => x.Telefono)
                .NotEmpty().WithMessage("El telefono es obligatorio.")
                .MaximumLength(20).WithMessage("El teléfono no puede tener más de 20 caracteres.");
            RuleFor(p => p.Estado)
                .NotNull().WithMessage("El campo Estado no debe ser nulo, favor verifique.")
                .Must(estado => estado == 'A' || estado == 'I').WithMessage("El campo Estado debe ser 'A' (Activo) o 'I' (Inactivo).");
        }
    }
}