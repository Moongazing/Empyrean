using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Delete;

public class DeleteUserOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
}
