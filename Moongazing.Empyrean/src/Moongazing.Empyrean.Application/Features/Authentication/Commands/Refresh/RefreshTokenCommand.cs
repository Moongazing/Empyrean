using Doing.Retail.Application.Features.Authentication.Commands.Refresh;
using Doing.Retail.Application.Features.Authentication.Rules;
using Doing.Retail.Application.Services.User;
using MediatR;
using Moongazing.Empyrean.Application.Services.Auth;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Refresh;

public class RefreshTokenCommand : IRequest<RefreshTokenResponse>,
    ITransactionalRequest, IIntervalRequest
{
    public string RefreshToken { get; set; } = default!;
    public string IpAddress { get; set; } = default!;
    public int Interval => 15;

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, RefreshTokenResponse>
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly AuthBusinessRules authBusinessRules;

        public RefreshTokenCommandHandler(IAuthService authService,
                                          IUserService userService,
                                          AuthBusinessRules authBusinessRules)
        {
            this.authService = authService;
            this.userService = userService;
            this.authBusinessRules = authBusinessRules;
        }

        public async Task<RefreshTokenResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            RefreshTokenEntity? refreshToken = await authService.GetRefreshTokenByToken(request.RefreshToken);

            await authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

            if (refreshToken!.Revoked != null)
            {
                await authService.RevokeDescendantRefreshTokens(
                    refreshToken,
                    request.IpAddress,
                    reason: $"Attempted reuse of revoked ancestor token: {refreshToken.Token}");
            }


            await authBusinessRules.RefreshTokenShouldBeActive(refreshToken);

            UserEntity? user = await userService.GetAsync(predicate: u => u.Id == refreshToken.UserId,
                                                          cancellationToken: cancellationToken);

            await authBusinessRules.UserShouldBeExistsWhenSelected(user);

            RefreshTokenEntity newRefreshToken = await authService.RotateRefreshToken(user: user!,
                                                                                      refreshToken,
                                                                                      request.IpAddress);

            RefreshTokenEntity addedRefreshToken = await authService.AddRefreshToken(newRefreshToken);

            AccessToken createdAccessToken = await authService.CreateAccessToken(user!);

            await authService.DeleteOldRefreshTokens(refreshToken.UserId);

            RefreshTokenResponse refreshedTokensResponse = new()
            {
                AccessToken = createdAccessToken,
                RefreshToken = addedRefreshToken
            };

            return refreshedTokensResponse;


        }
    }
}