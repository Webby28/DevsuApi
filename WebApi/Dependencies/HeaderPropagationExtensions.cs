using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WebApi.Dependencies;

public static class HeaderPropagationExtensions
{
    public static IServiceCollection AddHeaderPropagation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.TryAddEnumerable(ServiceDescriptor
            .Singleton<IHttpMessageHandlerBuilderFilter, HeaderPropagationMessageHandlerBuilderFilter>());
        return services;
    }
}

internal class HeaderPropagationMessageHandlerBuilderFilter : IHttpMessageHandlerBuilderFilter
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HeaderPropagationMessageHandlerBuilderFilter(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public Action<HttpMessageHandlerBuilder> Configure(Action<HttpMessageHandlerBuilder> next)
    {
        return builder =>
        {
            builder.AdditionalHandlers.Add(new HeaderPropagationMessageHandler(_contextAccessor));
            next(builder);
        };
    }
}

public class HeaderPropagationMessageHandler : DelegatingHandler
{
    private readonly IHttpContextAccessor _contextAccessor;

    public HeaderPropagationMessageHandler(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (_contextAccessor.HttpContext != null)
        {
            var headerValue = _contextAccessor.HttpContext.Response.Headers["x-correlation-id"];
            if (!StringValues.IsNullOrEmpty(headerValue))
            {
                request.Headers.TryAddWithoutValidation("x-correlation-id", (string[])headerValue);
            }
        }

        return base.SendAsync(request, cancellationToken);
    }
}