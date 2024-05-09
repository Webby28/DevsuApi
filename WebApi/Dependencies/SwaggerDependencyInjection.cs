using Asp.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Linq;

namespace WebApi.Dependencies
{
    public static class SwaggerDependencyInjection
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            var config = services.GetConfiguration();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            })
                //.AddMvc()
                .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddOpenApiDocument(options =>
            {
                options.PostProcess = document =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "api-devsu"
                    };
                    document.Servers.Clear();
                };
                // Añadir el procesador para eliminar el parámetro 'api-version' ya que se usa por ruta
                options.OperationProcessors.Add(new RemoveVersionParameterOperationProcessor());
                options.OperationProcessors.Add(new AddApiKeyHeaderProcessor(config));
            });
        }

        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            return configuration;
        }

        public class RemoveVersionParameterOperationProcessor : IOperationProcessor
        {
            public bool Process(OperationProcessorContext context)
            {
                // Remover el parámetro 'api-version' de la lista de parámetros si está presente
                var parameter = context.OperationDescription.Operation.Parameters
                    .FirstOrDefault(p => p.Name == "api-version" && p.Kind == OpenApiParameterKind.Query);

                if (parameter != null)
                {
                    context.OperationDescription.Operation.Parameters.Remove(parameter);
                }

                return true;
            }
        }

        public class AddApiKeyHeaderProcessor : IOperationProcessor
        {
            private readonly string _header;

            // Constructor estándar
            public AddApiKeyHeaderProcessor(IConfiguration configuration)
            {
                _header = configuration.GetValue<string>("ApiKeyConfiguration:Header")!;
            }

            public bool Process(OperationProcessorContext context)
            {
                context.OperationDescription.Operation.Parameters.Add(new OpenApiParameter
                {
                    Name = _header,
                    Kind = OpenApiParameterKind.Header,
                    Schema = new NJsonSchema.JsonSchema
                    {
                        Type = NJsonSchema.JsonObjectType.String,
                    },
                    Position = 1,
                    IsRequired = true,
                    Description = "ApiKey para poder autorizar peticion"
                });

                return true;
            }
        }
    }
}