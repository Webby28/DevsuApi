using Continental.API.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Core.Interfaces;
using WebApi.Infrastructure.Database;
using WebApi.Infrastructure.Database.Helpers;

namespace WebApi.Infrastructure.Infrastructure;

public static class Install
{
    public static IServiceCollection AgregarInfraestructura(this IServiceCollection services, IConfiguration configuration)
    {
        var config = services.BuildServiceProvider().GetService<IConfiguration>();

        services.AddDbContext<OracleDbContext>(o =>
            o.UseOracle(config.GetConnectionString("Oracle")));
        services.AddScoped<Func<TipoConexion, IApplicationDbContext>>(serviceProvider => key =>
        {
            return key switch
            {
                TipoConexion.Oracle => serviceProvider.GetRequiredService<OracleDbContext>(),
                _ => throw new ApplicationException("DbContext no encontrado")
            };
        });

        services.AddScoped<IAppDb, AppAppDb>();

        services.AddSingleton<IConnectionStringFactory, ConnectionStringFactory>();


        services.AddTransient<IClientePersonaRepository, ClientePersonaRepository>();

        services.AddHttpClient("ApiDevsu", options =>
        {
            options.BaseAddress = new Uri(config.GetConnectionString("ApiDevsu"));
        });

        return services;
    }
}