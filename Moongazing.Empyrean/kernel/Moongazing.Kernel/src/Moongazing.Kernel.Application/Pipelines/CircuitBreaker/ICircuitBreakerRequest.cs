namespace Moongazing.Kernel.Application.Pipelines.CircuitBreaker;

public interface ICircuitBreakerRequest
{
    int ExceptionsAllowedBeforeBreaking { get; }
    TimeSpan DurationOfBreak { get; }
}
