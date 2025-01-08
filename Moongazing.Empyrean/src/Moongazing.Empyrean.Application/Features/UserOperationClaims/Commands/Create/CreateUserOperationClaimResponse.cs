using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.UserOperationClaims.Commands.Create;

public class CreateUserOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }
}
