namespace Moongazing.Kernel.Application.Pipelines.RateLimit;

public interface IRateLimitedRequest
{
    int RequestLimit { get; }
    TimeSpan TimeWindow { get; }
}

