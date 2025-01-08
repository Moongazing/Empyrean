using Doing.Retail.Application.Features.Authentication.Rules;
using Doing.Retail.Application.Services.User;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Application.Services.AuthenticatorService;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.VerifyOtpAuthenticator;

public class VerifyOtpAuthenticatorCommand : IRequest, ITransactionalRequest
{
    public Guid UserId { get; set; } = default!;
    public string ActivationCode { get; set; } = default!;

    public class VerifyOtpAuthenticatorCommandHandler : IRequestHandler<VerifyOtpAuthenticatorCommand>
    {
        private readonly AuthBusinessRules authBusinessRules;
        private readonly IAuthenticatorService authenticatorService;
        private readonly IOtpAuthenticatorRepository otpAuthenticatorRepository;
        private readonly IUserService userService;

        public VerifyOtpAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
                                                    IAuthenticatorService authenticatorService,
                                                    IOtpAuthenticatorRepository otpAuthenticatorRepository,
                                                    IUserService userService)
        {
            this.authBusinessRules = authBusinessRules;
            this.authenticatorService = authenticatorService;
            this.otpAuthenticatorRepository = otpAuthenticatorRepository;
            this.userService = userService;
        }

        public async Task Handle(VerifyOtpAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            OtpAuthenticatorEntity? otpAuthenticator = await otpAuthenticatorRepository.GetAsync(
                predicate: e => e.UserId == request.UserId,
                cancellationToken: cancellationToken);

            await authBusinessRules.OtpAuthenticatorShouldBeExists(otpAuthenticator);

            UserEntity? user = await userService.GetAsync(predicate: u => u.Id == request.UserId,
                                                          cancellationToken: cancellationToken);

            await authBusinessRules.UserShouldBeExistsWhenSelected(user);

            var updatedOtpAuthenticator = otpAuthenticator!;
            updatedOtpAuthenticator.IsVerified = true;

            var updatedUser = user!;

            updatedUser.AuthenticatorType = AuthenticatorType.Otp;

            await authenticatorService.VerifyAuthenticatorCode(updatedUser, request.ActivationCode);

            await otpAuthenticatorRepository.UpdateAsync(updatedOtpAuthenticator,
                                                         cancellationToken);

            await userService.UpdateAsync(updatedUser);

        }
    }
}
