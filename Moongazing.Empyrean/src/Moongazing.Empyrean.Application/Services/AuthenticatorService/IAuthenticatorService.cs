using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Services.AuthenticatorService;

public interface IAuthenticatorService
{
    public Task<EmailAuthenticatorEntity> CreateEmailAuthenticator(UserEntity user);
    public Task<OtpAuthenticatorEntity> CreateOtpAuthenticator(UserEntity user);
    public Task<string> ConvertSecretKeyToString(byte[] secretKey);
    public Task SendAuthenticatorCode(UserEntity user);
    public Task VerifyAuthenticatorCode(UserEntity user, string authenticatorCode);
}

