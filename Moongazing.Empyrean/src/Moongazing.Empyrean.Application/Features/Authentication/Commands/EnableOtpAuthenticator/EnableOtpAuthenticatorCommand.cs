using Doing.Retail.Application.Features.Authentication.Commands.EnableOtpAuthenticator;
using Doing.Retail.Application.Features.Authentication.Rules;
using Doing.Retail.Application.Services.User;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Application.Services.AuthenticatorService;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.EnableOtpAuthenticator;

public class EnableOtpAuthenticatorCommand : IRequest<EnableOtpAuthenticatorResponse>,
    ILoggableRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid UserId { get; set; } = default!;
    public int Interval => 15;

    public class EnableOtpAuthenticatorCommandHandler : IRequestHandler<EnableOtpAuthenticatorCommand, EnableOtpAuthenticatorResponse>
    {
        private readonly AuthBusinessRules authBusinessRules;
        private readonly IAuthenticatorService authenticatorService;
        private readonly IOtpAuthenticatorRepository otpAuthenticatorRepository;
        private readonly IUserService userService;

        public EnableOtpAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
                                                    IAuthenticatorService authenticatorService,
                                                    IOtpAuthenticatorRepository otpAuthenticatorRepository,
                                                    IUserService userService)
        {
            this.authBusinessRules = authBusinessRules;
            this.authenticatorService = authenticatorService;
            this.otpAuthenticatorRepository = otpAuthenticatorRepository;
            this.userService = userService;
        }

        public async Task<EnableOtpAuthenticatorResponse> Handle(EnableOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await userService.GetAsync(predicate: u => u.Id == request.UserId,
                                                          cancellationToken: cancellationToken);

            await authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);

            OtpAuthenticatorEntity? doesExistOtpAuthenticator = await otpAuthenticatorRepository.GetAsync(
                predicate: o => o.UserId == request.UserId,
                cancellationToken: cancellationToken);

            await authBusinessRules.OtpAuthenticatorThatVerifiedShouldNotBeExists(doesExistOtpAuthenticator);

            if (doesExistOtpAuthenticator != null)
            {
                await otpAuthenticatorRepository.DeleteAsync(doesExistOtpAuthenticator, true, cancellationToken);
            }

            OtpAuthenticatorEntity newOtpAuthenticator = await authenticatorService.CreateOtpAuthenticator(user!);

            OtpAuthenticatorEntity addedOtpAuthenticator = await otpAuthenticatorRepository.AddAsync(
                newOtpAuthenticator,
                cancellationToken);

            EnableOtpAuthenticatorResponse enabledOtpAuthenticatorDto = new()
            {
                SecretKey = await authenticatorService.ConvertSecretKeyToString(addedOtpAuthenticator.SecretKey)
            };
            return enabledOtpAuthenticatorDto;
        }
    }
}