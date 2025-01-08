using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.Authentication.Commands.Refresh;

public class RefreshTokenResponse : IResponse
{
    public AccessToken? AccessToken { get; set; }
    public RefreshTokenEntity? RefreshToken { get; set; }

}


