using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.OperationClaims.Queries.GetListByDynamic;

public class GetListByDynamicOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

}
