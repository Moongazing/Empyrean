using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetList;

public class GetListUserOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }
    public string OperationClaimName { get; set; } = default!;
}
