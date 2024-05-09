using AspNetCore.Authentication.ApiKey;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Dependencies
{
    public static class AuthenticationServiceExtensions
    {
        public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)
                .AddApiKeyInHeader<WebApiApiKeyProvider>(options =>
                {
                    options.Realm = configuration["ApiKeyConfiguration:Realm"];
                    options.KeyName = configuration["ApiKeyConfiguration:Header"];
                });

            services.AddSingleton<IApiKeyProvider, WebApiApiKeyProvider>();

            return services;
        }
    }
}