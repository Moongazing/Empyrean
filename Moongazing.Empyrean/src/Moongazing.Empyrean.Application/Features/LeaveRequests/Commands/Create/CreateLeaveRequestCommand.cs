using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;
using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
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

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Create;

public class CreateLeaveRequestCommand : IRequest<CreateLeaveRequestResponse>,
    ILoggableRequest, ISecuredRequest, ICacheRemoverRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid EmployeeId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public LeaveType LeaveType { get; set; } = default!;
    public string Reason { get; set; } = default!;
    public bool IsApproved { get; set; }
    public string ApprovedBy { get; set; } = default!;
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => LeaveRequestMessages.SectionName;
    public string? CacheKey => null;
    public int Interval => 15;


    public class CreateLeaveRequestCommandHandler : IRequestHandler<CreateLeaveRequestCommand, CreateLeaveRequestResponse>
    {
        private readonly ILeaveRequestRepository leaveRequestRepository;
        private readonly IMapper mapper;
        private readonly LeaveRequestBusinessRules leaveRequestBusinessRules;
        private readonly EmployeeBusinessRules employeeBusinessRules;

        public CreateLeaveRequestCommandHandler(ILeaveRequestRepository leaveRequestRepository,
                                                IMapper mapper,
                                                LeaveRequestBusinessRules leaveRequestBusinessRules,
                                                EmployeeBusinessRules employeeBusinessRules)
        {
            this.leaveRequestRepository = leaveRequestRepository;
            this.mapper = mapper;
            this.leaveRequestBusinessRules = leaveRequestBusinessRules;
            this.employeeBusinessRules = employeeBusinessRules;
        }

        public async Task<CreateLeaveRequestResponse> Handle(CreateLeaveRequestCommand request, CancellationToken cancellationToken)
        {
            await employeeBusinessRules.EmployeeShouldBeExists(employeeId: request.EmployeeId);
            await leaveRequestBusinessRules.LeaveRequestCannotBeDuplicate(
                employeeId: request.EmployeeId,
                startDate: request.StartDate,
                endDate: request.EndDate);

            LeaveRequestEntity? leaveRequest = mapper.Map<LeaveRequestEntity>(request);

            LeaveRequestEntity addedBankDetail = await leaveRequestRepository.AddAsync(leaveRequest);

            CreateLeaveRequestResponse response = mapper.Map<CreateLeaveRequestResponse>(addedBankDetail);

            return response;
        }
    }

}
