using Microsoft.AspNetCore.Builder;

namespace Moongazing.Kernel.Localization.Middlewares;

public static class ApplicationBuilderLocalizationMiddlewareExtensions
{
    public static IApplicationBuilder UseResponseLocalization(this IApplicationBuilder builder,
                                                              string defaultLocale = "en")
    {
        return builder.UseMiddleware<LocalizationMiddleware>(defaultLocale);
    }
}