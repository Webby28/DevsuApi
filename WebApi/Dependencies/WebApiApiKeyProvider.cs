using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApi.Dependencies
{
    public class WebApiApiKeyProvider : IApiKeyProvider
    {
        private readonly ILogger<WebApiApiKeyProvider> _logger;
        public string ApiKey => _apiKey;
        private readonly string _apiKey;

        public WebApiApiKeyProvider(ILogger<WebApiApiKeyProvider> logger, IConfiguration configuration)
        {
            _logger = logger;
            _apiKey = configuration.GetValue<string>("ApiKeyConfiguration:key")!;

            if (_apiKey is null)
            {
                throw new ArgumentException("No se encuentra el ApiKey");
            }
        }

        public Task<IApiKey> ProvideAsync(string key)
        {
            try
            {
                if (key.Equals(_apiKey))
                {
                    _logger.LogDebug("Key valido");
                    return Task.FromResult<IApiKey>(new WebApiApiKey(key));
                }

                _logger.LogWarning("Key no valido");

                return Task.FromResult<IApiKey>(null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Mensaje: {mensaje}", ex.Message);
                throw;
            }
        }
    }

    public class WebApiApiKey : IApiKey
    {
        public string Key { get; }
        public string OwnerName { get; } = "WilliamsBaez";
        public IReadOnlyCollection<Claim>? Claims { get; }

        public WebApiApiKey(string key)
        {
            Key = key;
        }
    }
}