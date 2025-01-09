using Moongazing.Empyrean.Application.Features.Authentication.Constants;
using Moongazing.Empyrean.Application.Features.Employee.Constants;
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

namespace Moongazing.Empyrean.Application.Features.Employee.Rules;

public class EmployeeBusinessRules : BaseBusinessRules
{
    private readonly IEmployeeRepository employeeRepository;
    private readonly ILocalizationService localizationService;

    public EmployeeBusinessRules(IEmployeeRepository employeeRepository, ILocalizationService localizationService)
    {
        this.employeeRepository = employeeRepository;
        this.localizationService = localizationService;
    }

    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, EmployeeMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task EmployeeShouldBeExistsWhenSelected(EmployeeEntity? employee)
    {
        if (employee == null)
        {
            await LocalizedBusinessException(EmployeeMessages.EmployeeNotFound);
        }
    }

    public async Task EmployeeShouldBeExists(Guid employeeId)
    {
        var employee = await employeeRepository.AnyAsync(predicate: e => e.Id == employeeId, withDeleted: false);
        if (!employee)
        {
            await LocalizedBusinessException(EmployeeMessages.EmployeeNotFound);
        }
    }
}
