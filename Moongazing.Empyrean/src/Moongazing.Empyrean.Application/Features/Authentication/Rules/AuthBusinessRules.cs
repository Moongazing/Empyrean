using Moongazing.Empyrean.Application.Features.Authentication.Constants;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Rules;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Localization.Abstractions;
using Moongazing.Kernel.Security.Hashing;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;

namespace Doing.Retail.Application.Features.Authentication.Rules;

public class AuthBusinessRules : BaseBusinessRules
{
    private readonly IUserRepository userRepository;
    private readonly ILocalizationService localizationService;

    public AuthBusinessRules(IUserRepository userRepository, ILocalizationService localizationService)
    {
        this.userRepository = userRepository;
        this.localizationService = localizationService;
    }

    private async Task LocalizedBusinessException(string messageKey)
    {
        var message = await localizationService.GetLocalizedAsync(messageKey, AuthMessages.SectionName);
        throw new BusinessException(message);
    }

    public async Task EmailAuthenticatorShouldBeExists(EmailAuthenticatorEntity? emailAuthenticator)
    {
        if (emailAuthenticator is null)
        {
            await LocalizedBusinessException(AuthMessages.EMAIL_AUTHENTICATOR_DONT_EXISTS);
        }
    }

    public async Task OtpAuthenticatorShouldBeExists(OtpAuthenticatorEntity? otpAuthenticator)
    {
        if (otpAuthenticator is null)
        {
            await LocalizedBusinessException(AuthMessages.OTP_AUTHENTICATOR_DONT_EXISTS);
        }
    }

    public async Task OtpAuthenticatorThatVerifiedShouldNotBeExists(OtpAuthenticatorEntity? otpAuthenticator)
    {
        if (otpAuthenticator != null && otpAuthenticator.IsVerified)
        {
            await LocalizedBusinessException(AuthMessages.ALREADY_VERIFIED_OTP_AUTHENTICATOR_EXISTS);
        }
    }

    public async Task EmailAuthenticatorActivationKeyShouldBeExists(EmailAuthenticatorEntity emailAuthenticator)
    {
        if (emailAuthenticator.ActivationKey == null)
        {
            await LocalizedBusinessException(AuthMessages.EMAIL_ACTIVATION_KEY_DONT_EXISTS);
        }
    }

    public async Task UserShouldBeExistsWhenSelected(UserEntity? user)
    {
        if (user == null)
        {
            await LocalizedBusinessException(AuthMessages.USER_NOT_FOUND);
        }
    }

    public async Task UserShouldNotBeHaveAuthenticator(UserEntity user)
    {
        if (user.AuthenticatorType != AuthenticatorType.None)
        {
            await LocalizedBusinessException(AuthMessages.USER_HAVE_ALREADY_A_AUTHENTICATOR);
        }
    }

    public async Task RefreshTokenShouldBeExists(RefreshTokenEntity? refreshToken)
    {
        if (refreshToken == null)
        {
            await LocalizedBusinessException(AuthMessages.REFRESH_TOKEN_NOT_FOUND);
        }
    }

    public async Task RefreshTokenShouldBeActive(RefreshTokenEntity refreshToken)
    {
        if (refreshToken.Revoked != null || DateTime.UtcNow >= refreshToken.Expires)
        {
            await LocalizedBusinessException(AuthMessages.INVALID_REFRESH_TOKEN);
        }
    }

    public async Task UserEmailShouldBeNotExists(string email)
    {
        bool doesExists = await userRepository.AnyAsync(u => u.Email == email);
        if (doesExists)
        {
            await LocalizedBusinessException(AuthMessages.USER_EMAIL_ALREADY_EXISTS);
        }
    }

    public async Task UserPasswordAndConfirmPasswordShouldBeMatch(string password, string confirmPassword)
    {
        if (password != confirmPassword)
        {
            await LocalizedBusinessException(AuthMessages.PASSWORD_DONT_MATCH);
        }
    }
    public async Task UserPasswordShouldBeMatch(Guid id, string password)
    {
        UserEntity? user = await userRepository.GetAsync(u => u.Id == id, enableTracking: false);
        await UserShouldBeExistsWhenSelected(user);
        if (!HashingHelper.VerifyHash(password, user!.PasswordHash, user.PasswordSalt))
        {
            await LocalizedBusinessException(AuthMessages.PASSWORD_DONT_MATCH);
        }
    }
}
