using MimeKit;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Application.Services.AuthenticatorService;
using Moongazing.Kernel.CrossCuttingConcerns.Exceptions.Types;
using Moongazing.Kernel.Mailing;
using Moongazing.Kernel.Security.EmailAuthenticator;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;
using Moongazing.Kernel.Security.OtpAuthenticator;

namespace Doing.Retail.Application.Services.AuthenticatorService;

public class AuthenticatorService : IAuthenticatorService
{
    private readonly IEmailAuthenticatorHelper emailAuthenticatorHelper;
    private readonly IEmailAuthenticatorRepository emailAuthenticatorRepository;
    private readonly IMailService mailService;
    private readonly IOtpAuthenticatorHelper otpAuthenticatorHelper;
    private readonly IOtpAuthenticatorRepository otpAuthenticatorRepository;

    public AuthenticatorService(IEmailAuthenticatorHelper emailAuthenticatorHelper,
                                IEmailAuthenticatorRepository emailAuthenticatorRepository,
                                IMailService mailService,
                                IOtpAuthenticatorHelper otpAuthenticatorHelper,
                                IOtpAuthenticatorRepository otpAuthenticatorRepository)
    {
        this.emailAuthenticatorHelper = emailAuthenticatorHelper;
        this.emailAuthenticatorRepository = emailAuthenticatorRepository;
        this.mailService = mailService;
        this.otpAuthenticatorHelper = otpAuthenticatorHelper;
        this.otpAuthenticatorRepository = otpAuthenticatorRepository;
    }

    public async Task<EmailAuthenticatorEntity> CreateEmailAuthenticator(UserEntity user)
    {
        EmailAuthenticatorEntity emailAuthenticator =
            new()
            {
                UserId = user.Id,
                ActivationKey = await emailAuthenticatorHelper.CreateEmailActivationKey(),
                IsVerified = false
            };
        return emailAuthenticator;
    }

    public async Task<OtpAuthenticatorEntity> CreateOtpAuthenticator(UserEntity user)
    {
        OtpAuthenticatorEntity otpAuthenticator =
            new()
            {
                UserId = user.Id,
                SecretKey = await otpAuthenticatorHelper.GenerateSecretKeyAsync(),
                IsVerified = false
            };
        return otpAuthenticator;
    }

    public async Task<string> ConvertSecretKeyToString(byte[] secretKey)
    {
        string result = await otpAuthenticatorHelper.ConvertSecretKeyToStringAsync(secretKey);
        return result;
    }

    public async Task SendAuthenticatorCode(UserEntity user)
    {
        if (user.AuthenticatorType is AuthenticatorType.Email)
            await SendAuthenticatorCodeWithEmail(user);
    }

    public async Task VerifyAuthenticatorCode(UserEntity user, string authenticatorCode)
    {
        if (user.AuthenticatorType is AuthenticatorType.Email)
            await VerifyAuthenticatorCodeWithEmail(user, authenticatorCode);
        else if (user.AuthenticatorType is AuthenticatorType.Otp)
            await VerifyAuthenticatorCodeWithOtp(user, authenticatorCode);
    }

    private async Task SendAuthenticatorCodeWithEmail(UserEntity user)
    {
        EmailAuthenticatorEntity? emailAuthenticator = await emailAuthenticatorRepository.GetAsync(predicate: e => e.UserId == user.Id)
                                                       ?? throw new BusinessException("Email Authenticator not found.");

        if (!emailAuthenticator.IsVerified)
        {
            throw new BusinessException("Email Authenticator must be verified.");
        }

        string authenticatorCode = await emailAuthenticatorHelper.CreateEmailActivationCode();
        EmailAuthenticatorEntity updatedEmailAuthenticator = emailAuthenticator;
        updatedEmailAuthenticator.ActivationKey = authenticatorCode;

        await emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);

        var toEmailList = new List<MailboxAddress> { new($"{user.FirstName} {user.LastName}", user.Email) };


        mailService.SendMail(
            new Mail(
                subject: "Authenticator Code - Empyrean",
                textBody: $"Enter your authenticator code: {authenticatorCode}",
                htmlBody: string.Empty,
                toList: toEmailList
            )
        );
    }


    private async Task VerifyAuthenticatorCodeWithEmail(UserEntity user, string authenticatorCode)
    {
        EmailAuthenticatorEntity? emailAuthenticator = await emailAuthenticatorRepository.GetAsync(predicate: e => e.UserId == user.Id)
                                                       ?? throw new BusinessException("Email Authenticator not found.");

        if (!string.Equals(emailAuthenticator.ActivationKey, authenticatorCode, StringComparison.OrdinalIgnoreCase))
        {
            throw new BusinessException("Authenticator code is invalid.");
        }

        EmailAuthenticatorEntity updatedEmailAuthenticator = emailAuthenticator;
        updatedEmailAuthenticator.ActivationKey = null;

        await emailAuthenticatorRepository.UpdateAsync(emailAuthenticator);
    }

    private async Task VerifyAuthenticatorCodeWithOtp(UserEntity user, string authenticatorCode)
    {
        OtpAuthenticatorEntity? otpAuthenticator = await otpAuthenticatorRepository.GetAsync(predicate: e => e.UserId == user.Id)
                                                   ?? throw new BusinessException("Otp Authenticator not found.");
        bool result = await otpAuthenticatorHelper.VerifyCodeAsync(otpAuthenticator.SecretKey, authenticatorCode);
        if (!result)
            throw new BusinessException("Authenticator code is invalid.");
    }
}
