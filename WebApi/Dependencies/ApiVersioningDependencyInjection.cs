using Asp.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Dependencies
{
    public static class ApiVersioningDependencyInjection
    {
        public static IServiceCollection AgregarVersionamientoApi(
        this IServiceCollection services,
        int majorVersion,
        int minorVersion)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(majorVersion, minorVersion);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            }).AddMvc().AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}