using Moongazing.Empyrean.Application.Features.Users.Constans;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Rules;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Localization.Abstractions;
using Moongazing.Kernel.Security.Hashing;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.Users.Rules;

public class UserBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository userRepository;
    private readonly ILocalizationService localizationService;

    public UserBusinessRules(IUserRepository userRepository, ILocalizationService localizationService)
    {
        this.userRepository = userRepository;
        this.localizationService = localizationService;
    }

    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, UsersMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task UserShouldBeExistsWhenSelected(UserEntity? user)
    {
        if (user == null)
        {
            await LocalizedBusinessException(UsersMessages.USER_NOT_FOUND);
        }
    }


    public async Task UserIdShouldBeExistsWhenSelected(Guid id)
    {
        bool doesExist = await userRepository.AnyAsync(predicate: u => u.Id == id);

        if (!doesExist)
        {
            await LocalizedBusinessException(UsersMessages.USER_NOT_FOUND);
        }
    }

    public async Task UserPasswordShouldBeMatched(UserEntity user, string password)
    {
        if (!HashingHelper.VerifyHash(password, user.PasswordHash, user.PasswordSalt))
        {
            await LocalizedBusinessException(UsersMessages.PASSWORD_MISMATCH);
        }
    }

    public async Task UserEmailShouldNotExistsWhenInsert(string email)
    {
        bool doesExists = await userRepository.AnyAsync(predicate: u => u.Email == email);
        if (doesExists)
        {
            await LocalizedBusinessException(UsersMessages.EMAIL_ALREADY_EXISTS);
        }
    }

    public async Task UserEmailShouldNotExistsWhenUpdate(string email)
    {
        bool doesExists = await userRepository.AnyAsync(predicate: u => u.Email == email);
        if (doesExists)
        {
            await LocalizedBusinessException(UsersMessages.EMAIL_ALREADY_EXISTS);
        }
    }
}
