using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
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

namespace Moongazing.Empyrean.Application.Features.BankDetails.Rules;

public class BankDetailBusinessRules : BaseBusinessRules
{

    private readonly IBankDetailRepository bankDetailRepository;
    private readonly ILocalizationService localizationService;

    public BankDetailBusinessRules(IBankDetailRepository bankDetailRepository, ILocalizationService localizationService)
    {
        this.bankDetailRepository = bankDetailRepository;
        this.localizationService = localizationService;
    }

    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, BankDetailMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task BankDetailsShouldBeExistsWhenSelected(BankDetailEntity? bankDetails)
    {
        if (bankDetails == null)
        {
            await LocalizedBusinessException(BankDetailMessages.BankDetailNotFound);
        }
    }


    public async Task BankDetailsShouldBeExists(Guid bankDetailId)
    {
        var bankDetails = await bankDetailRepository.AnyAsync(predicate: e => e.Id == bankDetailId, withDeleted: false);
        if (!bankDetails)
        {
            await LocalizedBusinessException(BankDetailMessages.BankDetailNotFound);
        }
    }

    public async Task EmployeeShouldOneBankDetail(Guid employeeId)
    {
        var bankDetails = await bankDetailRepository.AnyAsync(predicate: e => e.EmployeeId == employeeId, withDeleted: false);
        if (bankDetails)
        {
            await LocalizedBusinessException(BankDetailMessages.BankDetailAlreadyExists);
        }
    }


    public async Task BankDetailsShouldBeUnique(string iban, string accountNumber)
    {
        var bankDetails = await bankDetailRepository.AnyAsync(predicate: e => e.IBAN == iban || e.AccountNumber == accountNumber, withDeleted: false);
        if (bankDetails)
        {
            await LocalizedBusinessException(BankDetailMessages.BankDetailAlreadyExists);
        }
    }
}
