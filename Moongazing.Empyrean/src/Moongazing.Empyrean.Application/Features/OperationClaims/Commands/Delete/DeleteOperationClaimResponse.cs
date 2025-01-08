using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.OperationClaims.Commands.Delete;

public class DeleteOperationClaimResponse : IResponse
{
    public Guid Id { get; set; }
}
