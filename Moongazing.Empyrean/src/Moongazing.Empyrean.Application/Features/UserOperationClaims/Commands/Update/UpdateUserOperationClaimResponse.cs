using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.UserOperationClaims.Commands.Update;

public class UpdateUserOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }
}
