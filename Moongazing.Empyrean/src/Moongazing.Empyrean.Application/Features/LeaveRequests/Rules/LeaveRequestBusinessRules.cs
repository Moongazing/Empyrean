using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Rules;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Localization.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Rules;

public class LeaveRequestBusinessRules : BaseBusinessRules
{
    private readonly ILeaveRequestRepository leaveRequestRepository;
    private readonly ILocalizationService localizationService;

    public LeaveRequestBusinessRules(ILeaveRequestRepository leaveRequestRepository,
                                     ILocalizationService localizationService)
    {
        this.leaveRequestRepository = leaveRequestRepository;
        this.localizationService = localizationService;
    }
    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, LeaveRequestMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task LeaveRequestShouldBeExists(LeaveRequestEntity? leaveRequest)
    {
        if (leaveRequest == null)
        {
            await LocalizedBusinessException(LeaveRequestMessages.LeaveRequestShouldBeExists);
        }
    }
    public async Task LeaveRequestShouldBeExists(Guid leaveRequestId)
    {
        var leaveRequest = await leaveRequestRepository.AnyAsync(predicate: x => x.Id == leaveRequestId,
                                                                 withDeleted: false);
        if (!leaveRequest)
        {
            await LocalizedBusinessException(LeaveRequestMessages.LeaveRequestShouldBeExists);
        }
    }

    public async Task LeaveRequestCannotBeDuplicate(Guid employeeId, DateTime startDate, DateTime endDate)
    {
        var leaveRequest = await leaveRequestRepository.AnyAsync(predicate: x => x.EmployeeId == employeeId ||
                                                                                 x.StartDate == startDate ||
                                                                                 x.EndDate == endDate,
                                                                                 withDeleted: false);

    }

}
