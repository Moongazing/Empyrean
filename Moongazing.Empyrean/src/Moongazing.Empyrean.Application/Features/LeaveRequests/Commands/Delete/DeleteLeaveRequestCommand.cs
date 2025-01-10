using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using static Moongazing.Empyrean.Application.Features.LeaveRequests.Constants.LeaveRequestOperationClaims;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Delete;

public class DeleteLeaveRequestCommand : IRequest<DeleteLeaveRequestResponse>,
    ILoggableRequest, ISecuredRequest, ICacheRemoverRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Write, LeaveRequestOperationClaims.Delete, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public string? CacheKey => null;
    public int Interval => 15;


    public class DeleteLeaveRequestCommandHandler : IRequestHandler<DeleteLeaveRequestCommand, DeleteLeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IMapper mapper;
        private readonly LeaveRequestBusinessRules leaveRequestBusinessRules;

        public DeleteLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                IMapper mapper,
                                                LeaveRequestBusinessRules leaveRequestBusinessRules)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.mapper = mapper;
            this.leaveRequestBusinessRules = leaveRequestBusinessRules;
        }

        public async Task<DeleteLeaveRequestResponse> Handle(DeleteLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveRequestEntity? leaveRequest = await leaveRequestRepository.GetAsync(
                predicate: e => e.Id == request.Id,
                withDeleted: false);

            await leaveRequestBusinessRules.LeaveRequestShouldBeExists(leaveRequest);

            await leaveRequestRepository.DeleteAsync(entity: leaveRequest!, permanent: true, cancellationToken: cancellationToken);

            DeleteLeaveRequestResponse response = mapper.Map<DeleteLeaveRequestResponse>(leaveRequest!);

            return response;
        }
    }
}

