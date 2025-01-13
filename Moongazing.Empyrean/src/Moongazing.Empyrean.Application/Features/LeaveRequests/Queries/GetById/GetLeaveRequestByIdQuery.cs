using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetPendingList;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.LeaveRequests.Constants.LeaveRequestOperationClaims;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetById;

public class GetLeaveRequestByIdQuery : IRequest<GetLeaveRequestByIdResponse>, ILoggableRequest, ICachableRequest, ISecuredRequest, IIntervalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({Id})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;

    public class GetLeaveRequestByIdQueryHandler : IRequestHandler<GetLeaveRequestByIdQuery, GetLeaveRequestByIdResponse>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IMapper mapper;
        private readonly LeaveRequestBusinessRules leaveRequestBusinessRules;

        public GetLeaveRequestByIdQueryHandler(ILeaveRequestRepository leaveRequestRepository,
                                               IMapper mapper,
                                               LeaveRequestBusinessRules leaveRequestBusinessRules)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.mapper = mapper;
            this.leaveRequestBusinessRules = leaveRequestBusinessRules;
        }

        public async Task<GetLeaveRequestByIdResponse> Handle(GetLeaveRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var leaveRequest = await leaveRequestRepository.GetAsync(predicate: x => x.Id == request.Id,
                                                                     include: x => x.Include(x => x.Employee),
                                                                     cancellationToken: cancellationToken);

            await leaveRequestBusinessRules.LeaveRequestShouldBeExists(leaveRequest);

            GetLeaveRequestByIdResponse response = mapper.Map<GetLeaveRequestByIdResponse>(leaveRequest);

            return response;



        }
    }
}
