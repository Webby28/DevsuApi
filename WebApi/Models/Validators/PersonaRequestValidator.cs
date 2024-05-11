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
                .MaximumLength(100).WithMessage("El nombre no puede tener más de 100 caracteres.")
                 .Matches("^[a-zA-Z ]+$").WithMessage("El nombre solo puede contener letras y espacios.");

            RuleFor(x => x.Genero)
                .NotEmpty().WithMessage("El género es obligatorio.")
                .Must(genero => genero.ToString().Length == 1).WithMessage("El campo genero debe tener exactamente un carácter.")
                .Must(genero => genero == "M" || genero == "F" || genero == "O").WithMessage("El campo Genero debe ser 'M' (Masculino), 'F' (Femenino), 'O' (Otro).");

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
                .Must(estado => estado.ToString().Length == 1).WithMessage("El campo Estado debe tener exactamente un carácter.")
                .Must(estado => estado == "A" || estado == "I").WithMessage("El campo Estado debe ser 'A' (Activo) o 'I' (Inactivo).");
        }
    }
}