using AutoMapper;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using WebApi.Core.Contracts.Helpers;
using WebApi.Core.Contracts.Requests;
using WebApi.Core.Interfaces;
using WebApi.Core.Services;
using WebApi.Models.Validators;

namespace WebApi.Test
{
    [TestFixture]
    public class ClientePersonaServiceTests
    {
        private ClientePersonaService _clientePersonaService;
        private Mock<ILogger<ClientePersonaService>> _loggerMock;
        private Mock<IClientePersonaRepository> _clientePersonaRepository;
        private Mock<IMapper> _mapperMock;
        private PersonaRequestValidator _personaRequestValidator;


        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<ClientePersonaService>>();
            _clientePersonaRepository = new Mock<IClientePersonaRepository>();
            _mapperMock = new Mock<IMapper>();
            _personaRequestValidator = new PersonaRequestValidator();
            _clientePersonaService = new ClientePersonaService(_clientePersonaRepository.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public void CrearPersona_Codigo_No_Valido_Retorna_Error_1(int codigo)
        {
            //Assert
            const int codigoError = 1;
            //Arrange
            var ex = Assert.ThrowsAsync<ReglaNegociosException>(async () => await _clientePersonaService.ObtenerCliente(codigo));

            //Assert
            Assert.That((int)ex.CodigoError, Is.EqualTo(codigoError));

        }

        [Test]
        public void PersonaRequestValidator_ValidarNombre_Vacio_DeberiaFallar()
        {
            // Arrange
            var personaRequest = new PersonaRequest { Nombre = "" };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Nombre).WithErrorMessage("El nombre es obligatorio.");
        }

        [Test]
        public void PersonaRequestValidator_ValidarGenero_Vacio_DeberiaFallar()
        {
            // Arrange
            var personaRequest = new PersonaRequest { Genero = "" };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Genero).WithErrorMessage("El género es obligatorio.");
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void PersonaRequestValidator_ValidarEdad_Menor_0_DeberiaFallar(int edad)
        {
            // Arrange
            var personaRequest = new PersonaRequest { Edad = edad };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Edad).WithErrorMessage("La edad debe ser mayor que cero.");
        }
        [Test]
        public void PersonaRequestValidator_ValidarDireccion_Vacio_DeberiaFallar()
        {
            // Arrange
            var personaRequest = new PersonaRequest { Direccion = "" };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Direccion).WithErrorMessage("La dirección es obligatoria.");
        }
    }
}
