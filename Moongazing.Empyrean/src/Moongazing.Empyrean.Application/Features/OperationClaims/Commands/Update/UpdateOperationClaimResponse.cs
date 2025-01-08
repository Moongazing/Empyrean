using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.OperationClaims.Commands.Update;

public class UpdateOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;


}
