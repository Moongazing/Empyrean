using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Login;

public class LoginResponse : IResponse
{
    public AccessToken? AccessToken { get; set; }
    public RefreshTokenEntity? RefreshToken { get; set; }
    public AuthenticatorType? RequiredAuthenticatorType { get; set; }

    public LogginHttpResponse ToHttpResponse() => new() { AccessToken = AccessToken, RequiredAuthenticatorType = RequiredAuthenticatorType };

    public class LogginHttpResponse
    {
        public AccessToken? AccessToken { get; set; }
        public AuthenticatorType? RequiredAuthenticatorType { get; set; }
    }
}