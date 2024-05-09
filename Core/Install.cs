using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Interfaces;
using WebApi.Core.Services;

namespace Core;

public static class Install
{
    public static IServiceCollection AgregarCore(this IServiceCollection services)
    {
        services.AddTransient<IClientePersonaService, ClientePersonaService>();
        return services;
    }
}