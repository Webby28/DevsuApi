using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Infrastructure.Database;

namespace WebApi.Dependencies;

public class HealthCheckResponse
{
    public string Status { get; set; }
    public IEnumerable<HealthCheck> Checks { get; set; }
    public TimeSpan Duration { get; set; }
}

public class HealthCheck
{
    public string Status { get; set; }
    public string Component { get; set; }
    public string Description { get; set; }
}

public static class Extension
{
    public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
    {
        var builder = services.AddHealthChecks();

        builder.AddCheck("self", () => HealthCheckResult.Healthy());
        builder.AddDbContextCheck<OracleDbContext>();

        return services;
    }

    public static async Task HealthChecksResponseWriter(HttpContext context, HealthReport report)
    {
        context.Response.ContentType = "application/json";

        var response = new HealthCheckResponse
        {
            Status = report.Status.ToString(),
            Checks = report.Entries.Select(x => new HealthCheck
            {
                Component = x.Key,
                Status = x.Value.Status.ToString(),
                Description = x.Value.Description
            }),
            Duration = report.TotalDuration
        };

        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
    }
}