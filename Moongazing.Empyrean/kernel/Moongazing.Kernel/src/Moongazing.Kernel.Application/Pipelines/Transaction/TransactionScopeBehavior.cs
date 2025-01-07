using MediatR;
using System.Transactions;

namespace Moongazing.Kernel.Application.Pipelines.Transaction;

public class TransactionScopeBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ITransactionalRequest
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {

        using TransactionScope transactionScope = new(TransactionScopeAsyncFlowOption.Enabled);

        try
        {
            TResponse response = await next();
            transactionScope.Complete();
            return response;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
