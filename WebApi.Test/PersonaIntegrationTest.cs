using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace WebApi.Test
{
    [TestFixture]
    public class PersonaIntegrationTest
    {
        private WebApplicationFactory<Program> _factory;
        private HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5000/v1/api/";

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = new WebApplicationFactory<Program>();
            _httpClient = _factory.CreateClient();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
            _factory.Dispose();
        }

        [Test]
        public async Task ConsultarCuenta_Inexistente_DeberiaRetornarStatusCode400()
        {
            // Arrange
            var idCuentaInexistente = 999999;
            var url = $"{BaseUrl}/cuenta/{idCuentaInexistente}";

            // Act
            var response = await _httpClient.GetAsync(url);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}