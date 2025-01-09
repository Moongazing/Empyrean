using Doing.Retail.Application.Services.Repositories;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Constants;
using Moongazing.Kernel.Application.Rules;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Localization.Abstractions;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Rules;

public class UserOperationClaimBusinessRules : BaseBusinessRules
{
    private readonly IUserOperationClaimRepository userOperationClaimRepository;
    private readonly ILocalizationService localizationService;

    public UserOperationClaimBusinessRules(IUserOperationClaimRepository userOperationClaimRepository,
                                           ILocalizationService localizationService)
    {
        this.userOperationClaimRepository = userOperationClaimRepository;
        this.localizationService = localizationService;
    }

    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, UserOperationClaimsMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task UserOperationClaimShouldExistWhenSelected(UserOperationClaimEntity? userOperationClaim)
    {
        if (userOperationClaim == null)
        {
            await LocalizedBusinessException(UserOperationClaimsMessages.USER_OPERATION_CLAIM_NOT_FOUND);
        }
    }

    public async Task UserOperationClaimIdShouldExistWhenSelected(Guid id)
    {
        bool doesExist = await userOperationClaimRepository.AnyAsync(predicate: b => b.Id == id);
        if (!doesExist)
        {
            await LocalizedBusinessException(UserOperationClaimsMessages.USER_OPERATION_CLAIM_NOT_FOUND);
        }
    }

    public async Task UserOperationClaimShouldNotExistWhenSelected(UserOperationClaimEntity? userOperationClaim)
    {
        if (userOperationClaim != null)
        {
            await LocalizedBusinessException(UserOperationClaimsMessages.USER_OPERATION_CLAIM_ALREADY_EXISTS);
        }
    }

    public async Task UserShouldNotHaveOperationClaimAlreadyWhenInsert(Guid userId, Guid operationClaimId)
    {
        bool doesExist = await userOperationClaimRepository.AnyAsync(u => u.UserId == userId && u.OperationClaimId == operationClaimId);
        if (doesExist)
        {
            await LocalizedBusinessException(UserOperationClaimsMessages.USER_OPERATION_CLAIM_ALREADY_EXISTS);
        }
    }

    public async Task UserShouldNotHaveOperationClaimAlreadyWhenUpdated(Guid id, Guid userId, Guid operationClaimId)
    {
        bool doesExist = await userOperationClaimRepository.AnyAsync(predicate: uoc =>
            uoc.Id == id && uoc.UserId == userId && uoc.OperationClaimId == operationClaimId
        );
        if (doesExist)
        {
            await LocalizedBusinessException(UserOperationClaimsMessages.USER_OPERATION_CLAIM_ALREADY_EXISTS);
        }
    }
}
