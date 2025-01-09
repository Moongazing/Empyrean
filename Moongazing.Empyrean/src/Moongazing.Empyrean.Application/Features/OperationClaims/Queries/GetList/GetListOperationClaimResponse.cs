using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetList;

public class GetListOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;

}