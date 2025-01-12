using MediatR;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.LeaveRequests.Constants.LeaveRequestOperationClaims;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetByLeaveTypeList;

public class GetLeaveRequestByTypeListQuery : IRequest<GetLeaveRequestByTypeListResponse>
{
    public LeaveType LeaveType { get; set; }
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({PageRequest.PageIndex}-{PageRequest.PageSize}/{LeaveType})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;
}
