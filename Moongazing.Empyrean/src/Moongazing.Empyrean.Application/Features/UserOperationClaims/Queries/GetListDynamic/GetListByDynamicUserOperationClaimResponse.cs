using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.UserOperationClaims.Queries.GetListDynamic;

public class GetListByDynamicUserOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }
    public string OperationClaimName { get; set; } = default!;
}
