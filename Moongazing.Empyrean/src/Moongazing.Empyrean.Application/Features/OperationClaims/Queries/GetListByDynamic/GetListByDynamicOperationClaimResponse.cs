using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetListByDynamic;

public class GetListByDynamicOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

}
