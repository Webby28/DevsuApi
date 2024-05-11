using FluentValidation;
using WebApi.Core.Contracts.Requests;

namespace WebApi.Models.Validators
{
    /// <summary>
    /// Valida los atributos de la clase ParametrosFluentValidation
    /// </summary>
    public class ModificarEstadoRequestValidator : AbstractValidator<ModificarEstadoRequest>
    {
        /// <summary>
        /// Método para validar los parametros para actualizacion de estado. 
        /// </summary>
        public ModificarEstadoRequestValidator()
        {
            RuleFor(p => p.Estado)
                .NotNull().WithMessage("El campo Estado no debe ser nulo, favor verifique.")
                .Must(estado => estado.ToString().Length == 1).WithMessage("El campo Estado debe tener exactamente un carácter.")
                .Must(estado => estado == "A" || estado == "I").WithMessage("El campo Estado debe ser 'A' (Activo) o 'I' (Inactivo).");
        }


    }
}