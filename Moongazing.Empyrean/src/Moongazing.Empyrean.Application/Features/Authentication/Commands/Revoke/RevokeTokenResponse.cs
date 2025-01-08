using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Revoke;

public class RevokeTokenResponse : IResponse
{
    public Guid Id { get; set; } = default!;
    public string Token { get; set; } = default!;
}