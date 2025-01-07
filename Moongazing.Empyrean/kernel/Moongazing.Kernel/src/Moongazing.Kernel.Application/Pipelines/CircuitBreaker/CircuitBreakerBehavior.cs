using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly;
using Polly.CircuitBreaker;

namespace Moongazing.Kernel.Application.Pipelines.CircuitBreaker;

public class CircuitBreakerBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICircuitBreakerRequest
{
    private readonly AsyncCircuitBreakerPolicy circuitBreakerPolicy;

    public CircuitBreakerBehavior()
    {
        circuitBreakerPolicy = Policy.Handle<Exception>().CircuitBreakerAsync(1, TimeSpan.FromSeconds(30));
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var dynamicPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                request.ExceptionsAllowedBeforeBreaking,
                request.DurationOfBreak
            );

        return await dynamicPolicy.ExecuteAsync(async () => await next());
    }
}
