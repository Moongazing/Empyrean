using MediatR;
using Microsoft.AspNetCore.Http;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Extensions;

namespace Moongazing.Kernel.Application.Pipelines.Authorization;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, ISecuredRequest
{
    private readonly IHttpContextAccessor httpContextAccessor;
    public AuthorizationBehavior(IHttpContextAccessor httpContextAccessor)
    {
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!httpContextAccessor.HttpContext.User.Claims.Any())
            throw new AuthorizationException("You are not authenticated.");

        if (request.Roles.Length != 0)
        {
            ICollection<string>? userRoleClaims = httpContextAccessor.HttpContext.User.GetRoleClaims() ?? [];
            bool isMatchedAUserRoleClaimWithRequestRoles = userRoleClaims.Any(userRoleClaim => userRoleClaim == GeneralOperationClaims.Admin || request.Roles.Contains(userRoleClaim));
            if (!isMatchedAUserRoleClaimWithRequestRoles)
            {
                throw new AuthorizationException("You are not authorized.");
            }
        }

        TResponse response = await next();
        return response;
    }
}
