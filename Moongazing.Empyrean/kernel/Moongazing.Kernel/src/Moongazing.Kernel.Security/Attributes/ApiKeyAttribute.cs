using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;

namespace Moongazing.Kernel.Security.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetService<IConfiguration>();

        var validApiKey = configuration?["ApiKey:Value"] ?? string.Empty;
        var apiKeyHeaderName = configuration?["ApiKey:Key"] ?? "X-Token";

        if (!context.HttpContext.Request.Headers.TryGetValue(apiKeyHeaderName, out var extractedApiKey))
        {
            throw new AuthorizationException();
        }

        if (!string.Equals(extractedApiKey, validApiKey, StringComparison.Ordinal))
        {
            throw new AuthorizationException();

        }
    }
}
