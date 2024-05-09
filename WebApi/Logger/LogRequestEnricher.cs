using Microsoft.AspNetCore.Http;
using Serilog;
using System.Linq;

namespace WebApi.Logger;

public static class LogRequestEnricher
{
    public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
    {
        diagnosticContext.Set("ClientIp", httpContext.Connection.RemoteIpAddress);
        diagnosticContext.Set("Host", httpContext.Request.Host);
        diagnosticContext.Set("Path", httpContext.Request.Path);
        diagnosticContext.Set("Metodo", httpContext.Request.Method);
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
    }
}