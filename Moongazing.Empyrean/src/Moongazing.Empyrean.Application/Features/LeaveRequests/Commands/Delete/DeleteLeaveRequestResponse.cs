using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Delete;

public class DeleteLeaveRequestResponse : IResponse
{

    public Guid Id { get; set; }
    public string DeletedBy { get; set; } = default!;
    public DateTime DeletedAt { get; set; }
}