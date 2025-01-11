using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetTodayList;

public class GetLeaveRequestListByTodayResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveType LeaveType { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public bool IsApproved { get; set; }
    public string ApprovedBy { get; set; } = default!;
}