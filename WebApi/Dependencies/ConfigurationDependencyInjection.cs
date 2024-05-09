using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Dependencies
{
    public static class ConfigurationDependencyInjection
    {
        public static IServiceCollection AgregarConfiguraciones(this IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }
    }
}