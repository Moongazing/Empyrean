using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.UserOperationClaims.Commands.Delete;

public class DeleteUserOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
}
