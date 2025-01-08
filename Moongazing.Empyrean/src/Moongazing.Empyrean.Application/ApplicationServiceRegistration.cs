using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.CircuitBreaker;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.RateLimit;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Application.Pipelines.Validation;
using Moongazing.Kernel.Application.Rules;
using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.ConfigurationModels;
using Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.Logger;
using Moongazing.Kernel.Localization;
using Moongazing.Kernel.Mailing;
using Moongazing.Kernel.Mailing.MailKitImplementations;
using Moongazing.Kernel.Security.Jwt;
using System.Reflection;
using ILogger = Moongazing.Kernel.CrossCuttingConcerns.Logging.Serilog.ILogger;


namespace Moongazing.Empyrean.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
                                                            MailSettings mailSettings,
                                                            FileLogConfiguration fileLogConfiguration,
                                                            TokenOptions tokenOptions)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            configuration.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
            configuration.AddOpenBehavior(typeof(CachingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CacheRemovingBehavior<,>));
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            configuration.AddOpenBehavior(typeof(TransactionScopeBehavior<,>));
            configuration.AddOpenBehavior(typeof(RateLimitingBehavior<,>));
            configuration.AddOpenBehavior(typeof(CircuitBreakerBehavior<,>));


        });

        services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMailService, MailKitMailService>(_ => new MailKitMailService(mailSettings));
        services.AddSingleton<ILogger, SeriLogFileLogger>(_ => new SeriLogFileLogger(fileLogConfiguration));

        services.AddYamlResourceLocalization();


        return services;
    }

    public static IServiceCollection AddSubClassesOfType(this IServiceCollection services,
                                                         Assembly assembly,
                                                         Type type,
                                                         Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null)
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (Type? item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);
            else
                addWithLifeCycle(services, type);
        return services;
    }
}