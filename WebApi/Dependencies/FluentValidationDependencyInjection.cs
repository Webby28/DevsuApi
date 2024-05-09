using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace WebApi.Dependencies;

public static class FluentValidationDependencyInjection
{
    public static IServiceCollection AgregarFluentValidation(this IServiceCollection services)
    {
        services.AddFluentValidationAutoValidation(config =>
        {
            config.DisableDataAnnotationsValidation = true;
        });
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddTransient<IValidatorInterceptor, ValidatorInterceptor>();

        return services;
    }
}

public class ValidatorInterceptor : IValidatorInterceptor
{
    private readonly ILogger<ValidatorInterceptor> _logger;

    public ValidatorInterceptor(ILogger<ValidatorInterceptor> logger)
    {
        _logger = logger;
    }

    public IValidationContext BeforeAspNetValidation(ActionContext actionContext, IValidationContext commonContext)
    {
        return commonContext;
    }

    public ValidationResult AfterAspNetValidation(ActionContext actionContext, IValidationContext validationContext,
        ValidationResult result)
    {
        try
        {
            if (!result.IsValid)
            {
                var model = validationContext.InstanceToValidate;
                var controller = $"{actionContext.ActionDescriptor.DisplayName}";
                _logger.LogWarning("Modelo invalido en {controller} {@Request}", controller, model);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error en ValidatorInterceptor");
        }

        return result;
    }
}