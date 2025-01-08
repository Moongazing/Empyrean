using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;

public class CreateOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;


}