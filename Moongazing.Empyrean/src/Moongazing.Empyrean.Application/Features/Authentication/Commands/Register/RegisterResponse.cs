using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Register;

public class RegisterResponse : IResponse
{
    public AccessToken AccessToken { get; set; } = default!;
    public RefreshTokenEntity RefreshToken { get; set; } = default!;
}
