using Doing.Retail.Application.Features.OperationClaims.Constants;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Rules;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Localization.Abstractions;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.OperationClaims.Rules;

public class OperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IOperationClaimRepository operationClaimRepository;
    private readonly ILocalizationService localizationService;

    public OperationClaimBusinessRules(IOperationClaimRepository operationClaimRepository, ILocalizationService localizationService)
    {
        this.operationClaimRepository = operationClaimRepository;
        this.localizationService = localizationService;
    }

    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, OperationClaimsMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task OperationClaimShouldExistWhenSelected(OperationClaimEntity? operationClaim)
    {
        if (operationClaim is null)
        {
            await LocalizedBusinessException(OperationClaimsMessages.OPERATION_CLAIM_NOT_FOUND);
        }
    }

    public async Task OperationClaimIdShouldExistWhenSelected(Guid id)
    {
        bool doesExist = await operationClaimRepository.AnyAsync(predicate: b => b.Id == id);
        if (!doesExist)
        {
            await LocalizedBusinessException(OperationClaimsMessages.OPERATION_CLAIM_NOT_FOUND);
        }
    }

    public async Task OperationClaimNameShouldNotExistWhenCreating(string name)
    {
        bool doesExist = await operationClaimRepository.AnyAsync(predicate: b => b.Name == name);
        if (doesExist)
        {
            await LocalizedBusinessException(OperationClaimsMessages.OPERATION_CLAIM_ALREADY_EXISTS);
        }
    }

    public async Task OperationClaimNameShouldNotExistWhenUpdating(Guid id, string name)
    {
        bool doesExist = await operationClaimRepository.AnyAsync(predicate: b => b.Id != id && b.Name == name);
        if (doesExist)
        {
            await LocalizedBusinessException(OperationClaimsMessages.OPERATION_CLAIM_ALREADY_EXISTS);
        }
    }
}
