using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Features.BankDetail.Commands.Update;
using Moongazing.Empyrean.Application.Features.Employee.Rules;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Empyrean.Domain.Enums;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.LeaveRequests.Constants.LeaveRequestOperationClaims;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Update;

public class UpdateLeaveRequestCommand : IRequest<UpdateLeaveRequestResponse>,
    ILoggableRequest, ISecuredRequest, ICacheRemoverRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveType LeaveType { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public bool IsApproved { get; set; }
    public string ApprovedBy { get; set; } = default!;
    public string[] Roles => [Admin, Write, LeaveRequestOperationClaims.Update, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public string? CacheKey => null;
    public int Interval => 15;

    public class UpdateLeaveRequestCommandHandler : IRequestHandler<UpdateLeaveRequestCommand, UpdateLeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IMapper mapper;
        private readonly EmployeeBusinessRules employeeBusinessRules;
        private readonly LeaveRequestBusinessRules leaveRequestBusinessRules;

        public UpdateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                IMapper mapper,
                                                EmployeeBusinessRules employeeBusinessRules,
                                                LeaveRequestBusinessRules leaveRequestBusinessRules)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.mapper = mapper;
            this.employeeBusinessRules = employeeBusinessRules;
            this.leaveRequestBusinessRules = leaveRequestBusinessRules;
        }

        public async Task<UpdateLeaveRequestResponse> Handle(UpdateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            LeaveRequestEntity? leaveRequest = await leaveRequestRepository.GetAsync(
               predicate: x => x.Id == request.Id,
               cancellationToken: cancellationToken);

            await leaveRequestBusinessRules.LeaveRequestShouldBeExists(leaveRequest!);
            await employeeBusinessRules.EmployeeShouldBeExists(request.EmployeeId);

            var updatedLeaveRequest = mapper.Map(request, leaveRequest);

            var result = await leaveRequestRepository.UpdateAsync(updatedLeaveRequest!, cancellationToken);

            UpdateLeaveRequestResponse response = mapper.Map<UpdateLeaveRequestResponse>(result);

            return response;
        }
    }
}
