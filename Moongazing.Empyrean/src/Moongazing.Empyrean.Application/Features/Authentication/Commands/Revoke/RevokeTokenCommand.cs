using AutoMapper;
using Doing.Retail.Application.Features.Authentication.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Services.Auth;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Models;
using static Doing.Retail.Application.Features.Authentication.Constants.AuthOperationClaims;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Revoke;

public class RevokeTokenCommand : IRequest<RevokeTokenResponse>,
    ISecuredRequest, ILoggableRequest, ITransactionalRequest, IIntervalRequest
{
    public string Token { get; set; } = default!;
    public string IpAddress { get; set; } = default!;
    public string[] Roles => [Admin, RevokeToken];
    public int Interval => 15;


    public class RevokeTokenCommandHandler : IRequestHandler<RevokeTokenCommand, RevokeTokenResponse>
    {
        private readonly IAuthService authService;
        private readonly AuthBusinessRules authBusinessRules;
        private readonly IMapper mapper;

        public RevokeTokenCommandHandler(IAuthService authService,
                                         AuthBusinessRules authBusinessRules,
                                         IMapper mapper)
        {
            this.authService = authService;
            this.authBusinessRules = authBusinessRules;
            this.mapper = mapper;
        }

        public async Task<RevokeTokenResponse> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            RefreshTokenEntity? refreshToken = await authService.GetRefreshTokenByToken(request.Token);

            await authBusinessRules.RefreshTokenShouldBeExists(refreshToken);

            await authBusinessRules.RefreshTokenShouldBeActive(refreshToken!);

            await authService.RevokeRefreshToken(token: refreshToken!,
                                                 request.IpAddress,
                                                 replacedByToken: null,
                                                 reason: "Revoked without replacement");

            RevokeTokenResponse revokedTokenResponse = mapper.Map<RevokeTokenResponse>(refreshToken!);

            return revokedTokenResponse;
        }
    }
}