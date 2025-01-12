using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Features.Employee.Rules;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetTodayList;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.LeaveRequests.Constants.LeaveRequestOperationClaims;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetByEmployeeId;

public class GetLeaveRequestByEmployeeListQuery : IRequest<GetListResponse<GetLeaveRequestByEmployeeResponse>>,
     ILoggableRequest, ICachableRequest, ISecuredRequest, IIntervalRequest
{
    public Guid EmployeeId { get; set; }
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({PageRequest.PageIndex}-{PageRequest.PageSize})/({EmployeeId})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;



    public class GetLeaveRequestByEmployeeListQueryHandler : IRequestHandler<GetLeaveRequestByEmployeeListQuery, GetListResponse<GetLeaveRequestByEmployeeResponse>>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IMapper mapper;
        private readonly LeaveRequestBusinessRules leaveRequestBusinessRules;
        private readonly EmployeeBusinessRules employeeBusinessRules;

        public GetLeaveRequestByEmployeeListQueryHandler(ILeaveRequestRepository leaveRequestRepository,
                                                         IMapper mapper,
                                                         LeaveRequestBusinessRules leaveRequestBusinessRules,
                                                         EmployeeBusinessRules employeeBusinessRules)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.mapper = mapper;
            this.leaveRequestBusinessRules = leaveRequestBusinessRules;
            this.employeeBusinessRules = employeeBusinessRules;
        }

        public async Task<GetListResponse<GetLeaveRequestByEmployeeResponse>> Handle(GetLeaveRequestByEmployeeListQuery request, CancellationToken cancellationToken)
        {
            await employeeBusinessRules.EmployeeShouldBeExists(request.EmployeeId);

            IPagebale<LeaveRequestEntity> leaveRequestList = await leaveRequestRepository.GetListAsync(
               predicate: x => x.EmployeeId == request.EmployeeId,
               index: request.PageRequest.PageIndex,
               size: request.PageRequest.PageSize,
               include: x => x.Include(x => x.Employee),
               cancellationToken: cancellationToken);

            GetListResponse<GetLeaveRequestByEmployeeResponse> response = mapper.Map<GetListResponse<GetLeaveRequestByEmployeeResponse>>(leaveRequestList);

            return response;
        }
    }


}
