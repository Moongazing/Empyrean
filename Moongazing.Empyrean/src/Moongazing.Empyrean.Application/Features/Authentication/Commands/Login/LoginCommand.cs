using Doing.Retail.Application.Features.Authentication.Rules;
using Doing.Retail.Application.Services.User;
using MediatR;
using Moongazing.Empyrean.Application.Services.Auth;
using Moongazing.Empyrean.Application.Services.AuthenticatorService;
using Moongazing.Kernel.Application.Dtos;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Security.Jwt;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Login;

public class LoginCommand : IRequest<LoginResponse>, ILoggableRequest, IIntervalRequest
{
    public UserLoginDto UserLoginDto { get; set; } = default!;
    public string IpAddress { get; set; } = default!;
    public int Interval => 15;

    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly AuthBusinessRules authBusinessRules;
        private readonly IAuthenticatorService authenticatorService;
        private readonly IAuthService authService;
        private readonly IUserService userService;

        public LoginCommandHandler(IUserService userService,
                                   IAuthService authService,
                                   AuthBusinessRules authBusinessRules,
                                   IAuthenticatorService authenticatorService)
        {
            this.userService = userService;
            this.authService = authService;
            this.authBusinessRules = authBusinessRules;
            this.authenticatorService = authenticatorService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await userService.GetAsync(predicate: u => u.Email == request.UserLoginDto.Email,
                                                          cancellationToken: cancellationToken);

            await authBusinessRules.UserShouldBeExistsWhenSelected(user);

            await authBusinessRules.UserPasswordShouldBeMatch(user!.Id, request.UserLoginDto.Password);

            LoginResponse loggedResponse = new();

            if (user.AuthenticatorType is not AuthenticatorType.None)
            {
                if (request.UserLoginDto.AuthenticatorCode == null)
                {
                    await authenticatorService.SendAuthenticatorCode(user);
                    return loggedResponse;
                }

                await authenticatorService.VerifyAuthenticatorCode(user, request.UserLoginDto.AuthenticatorCode);
            }
            await authService.DeleteOldRefreshTokens(user.Id);

            AccessToken createdAccessToken = await authService.CreateAccessToken(user);

            RefreshTokenEntity createdRefreshToken = await authService.CreateRefreshToken(user, request.IpAddress);

            RefreshTokenEntity addedRefreshToken = await authService.AddRefreshToken(createdRefreshToken);

            loggedResponse.AccessToken = createdAccessToken;
            loggedResponse.RefreshToken = addedRefreshToken;

            return loggedResponse;
        }
    }
}
