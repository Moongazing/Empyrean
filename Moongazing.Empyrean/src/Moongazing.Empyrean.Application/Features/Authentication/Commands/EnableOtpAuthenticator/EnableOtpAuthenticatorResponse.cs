using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.Authentication.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorResponse : IResponse
{
    public string SecretKey { get; set; } = default!;


}