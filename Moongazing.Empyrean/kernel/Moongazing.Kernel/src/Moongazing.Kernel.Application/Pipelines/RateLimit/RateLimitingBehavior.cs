using MediatR;
using Microsoft.Extensions.Caching.Memory;

namespace Moongazing.Kernel.Application.Pipelines.RateLimit;

public class RateLimitingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRateLimitedRequest
{
    private readonly IMemoryCache cache;

    public RateLimitingBehavior(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var key = $"RateLimit:{typeof(TRequest).Name}";
        var limit = request.RequestLimit;
        var timeWindow = request.TimeWindow;

        var requestCount = cache.GetOrCreate(key, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = timeWindow;
            return 0;
        });

        if (requestCount >= limit)
        {
            throw new Exception("Rate limit exceeded. Please try again later.");
        }

        cache.Set(key, requestCount + 1);
        return await next();
    }
}

