using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorResponse : IResponse
{
    public string SecretKey { get; set; } = default!;


}