using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetList;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Domain.Enums;
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

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetByLeaveTypeList;

public class GetLeaveRequestByTypeListQuery : IRequest<GetListResponse<GetLeaveRequestByTypeListResponse>>,
    ILoggableRequest, ICachableRequest, ISecuredRequest, IIntervalRequest
{
    public LeaveType LeaveType { get; set; }
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({PageRequest.PageIndex}-{PageRequest.PageSize}/{LeaveType})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;


    public class GetLeaveRequestByTypeListQueryHandler : IRequestHandler<GetLeaveRequestByTypeListQuery, GetListResponse<GetLeaveRequestByTypeListResponse>>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IMapper mapper;
        public GetLeaveRequestByTypeListQueryHandler(ILeaveRequestRepository leaveRequestRepository, IMapper mapper)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.mapper = mapper;
        }
        public async Task<GetListResponse<GetLeaveRequestByTypeListResponse>> Handle(GetLeaveRequestByTypeListQuery request, CancellationToken cancellationToken)
        {
            IPagebale<LeaveRequestEntity> leaveRequests = await leaveRequestRepository.GetListAsync(predicate: x => x.LeaveType == request.LeaveType,
                                                                                                    index: request.PageRequest.PageIndex,
                                                                                                    size: request.PageRequest.PageSize,
                                                                                                    include: x => x.Include(x => x.Employee),
                                                                                                    cancellationToken: cancellationToken);

            GetListResponse<GetLeaveRequestByTypeListResponse> response = mapper.Map<GetListResponse<GetLeaveRequestByTypeListResponse>>(leaveRequests);

            return response;



        }
    }



}
