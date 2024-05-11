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
    public class PersonaUnitTest
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
        public void PersonaRequestValidator_ValidarTelefono_LongitudMaxima_DeberiaFallar()
        {
            // Arrange
            var personaRequest = new PersonaRequest { Nombre = "Test", Genero = "O", Edad = 1, Identificacion = "123", Direccion = "Test", Telefono = "123456789012345678901" };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Telefono).WithErrorMessage("El teléfono no puede tener más de 20 caracteres.");
        }

        [Test]
        public void PersonaRequestValidator_ValidarGenero_Distinto_DeberiaFallar()
        {
            // Arrange
            var personaRequest = new PersonaRequest { Nombre = "Test", Genero = "S",  Edad = 1, Identificacion = "123", Direccion = "Test", Telefono = "Test" };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Genero).WithErrorMessage("El campo Genero debe ser 'M' (Masculino), 'F' (Femenino), 'O' (Otro).");
        }

        [TestCase(-10)]
        [TestCase(-1)]
        public void PersonaRequestValidator_ValidarEdad_Menor_0_DeberiaFallar(int edad)
        {
            // Arrange
            var personaRequest = new PersonaRequest { Nombre = "Test", Genero = "O", Edad = edad, Identificacion = "123", Direccion = "Test", Telefono = "123456789012345678901" };

            // Act
            var result = _personaRequestValidator.TestValidate(personaRequest);

            // Assert
            result.ShouldHaveValidationErrorFor(p => p.Edad).WithErrorMessage("La edad debe ser mayor que cero.");
        }       
    }
}