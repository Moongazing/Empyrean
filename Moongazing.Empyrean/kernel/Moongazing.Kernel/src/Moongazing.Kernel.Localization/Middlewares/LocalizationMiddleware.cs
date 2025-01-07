using Microsoft.AspNetCore.Http;
using Microsoft.Net.Http.Headers;
using Moongazing.Kernel.Localization.Abstractions;
using System.Collections.Immutable;

namespace Moongazing.Kernel.Localization.Middlewares;

public class LocalizationMiddleware
{
    private readonly RequestDelegate next;
    private readonly string defaultLocale;

    public LocalizationMiddleware(RequestDelegate next, string defaultLocale = "en")
    {
        this.next = next ?? throw new ArgumentNullException(nameof(next));
        this.defaultLocale = defaultLocale ?? throw new ArgumentNullException(nameof(defaultLocale));
    }

    public async Task InvokeAsync(HttpContext context, ILocalizationService localizationService)
    {
        ArgumentNullException.ThrowIfNull(localizationService);

        var acceptLanguages = context.Request.GetTypedHeaders().AcceptLanguage;
        localizationService.AcceptLocales = ExtractAcceptLanguages(acceptLanguages, defaultLocale);

        await next(context);
    }

    private static ImmutableArray<string> ExtractAcceptLanguages(
        IList<StringWithQualityHeaderValue>? acceptLanguages,
        string defaultLocale)
    {
        if (acceptLanguages != null && acceptLanguages.Any())
        {
            return acceptLanguages
                .OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .ToImmutableArray();
        }

        return [defaultLocale];
    }
}
