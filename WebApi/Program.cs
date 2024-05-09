using Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Serilog;
using System;
using System.Net;
using WebApi.Infrastructure.Infrastructure;
using WebApi.Dependencies;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                // Serilog
                builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

                Serilog.Debugging.SelfLog.Enable(Console.Error);

                Log.Information("Iniciando {ApplicationName}", builder.Configuration["Serilog:Properties:ApplicationName"]);

                // CONTENIDO DEL METODO Configure
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                builder.Services.AgregarConfiguraciones(builder.Configuration)
                    .AddHttpContextAccessor()
                    .AddHealthChecks(builder.Configuration)
                    .AgregarCore()
                    .AgregarInfraestructura(builder.Configuration)
                    .AgregarVersionamientoApi(1, 0)
                    .AgregarAutoMapper()
                    .AgregarFluentValidation();

                // Configuración de MVC y formateadores XML
                builder.Services.AddControllers()
                    .AddXmlSerializerFormatters()  // Añadir formateadores de XML al MVC Builder
                    .AddXmlDataContractSerializerFormatters(); 


                builder.Services.AddMvc()
                    .AddXmlSerializerFormatters()
                    .AddXmlDataContractSerializerFormatters();

                // Configuraciones adicionales
                builder.Services.AddApiKeyAuthentication(builder.Configuration)
                    .AddSwaggerConfiguration();

                IdentityModelEventSource.ShowPII = true;
                builder.Services.AddRouting(options => options.LowercaseUrls = true);

                var app = builder.Build();

                if (app.Environment.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();

                    System.Net.Http.HttpClient.DefaultProxy = new System.Net.WebProxy();
                }
                app.UseOpenApi();
                app.UseSwaggerUi(c =>
                {
                    c.DocumentPath = "/swagger/{documentName}/swagger.json";
                });

                app.UseReDoc(options =>
                {
                    options.Path = "/redoc";
                });

                app.UseRouting();

                app.UseAuthorization();

                app.MapControllers();

                app.MapHealthChecks("/readiness", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = Extension.HealthChecksResponseWriter
                });
                app.MapHealthChecks("/liveness", new HealthCheckOptions
                {
                    Predicate = r => r.Name.Contains("self"),
                });

                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "La API fallo al iniciar");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}