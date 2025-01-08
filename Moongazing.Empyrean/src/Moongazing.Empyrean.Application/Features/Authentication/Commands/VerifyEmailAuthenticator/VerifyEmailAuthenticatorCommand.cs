using Doing.Retail.Application.Features.Authentication.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.VerifyEmailAuthenticator;

public class VerifyEmailAuthenticatorCommand : IRequest, ILoggableRequest, ITransactionalRequest
{
    public string ActivationKey { get; set; } = default!;

    public class VerifyEmailAuthenticatorCommandHandler : IRequestHandler<VerifyEmailAuthenticatorCommand>
    {
        private readonly AuthBusinessRules authBusinessRules;
        private readonly IEmailAuthenticatorRepository emailAuthenticatorRepository;

        public VerifyEmailAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
                                                      IEmailAuthenticatorRepository emailAuthenticatorRepository)
        {
            this.authBusinessRules = authBusinessRules;
            this.emailAuthenticatorRepository = emailAuthenticatorRepository;
        }

        public async Task Handle(VerifyEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            EmailAuthenticatorEntity? emailAuthenticator = await emailAuthenticatorRepository.GetAsync(
                predicate: e => e.ActivationKey == request.ActivationKey,
                cancellationToken: cancellationToken);

            await authBusinessRules.EmailAuthenticatorShouldBeExists(emailAuthenticator);

            await authBusinessRules.EmailAuthenticatorActivationKeyShouldBeExists(emailAuthenticator!);

            var updatedEmailAuthenticator = emailAuthenticator!;

            updatedEmailAuthenticator.ActivationKey = null;
            updatedEmailAuthenticator.IsVerified = true;

            await emailAuthenticatorRepository.UpdateAsync(updatedEmailAuthenticator,
                                                           cancellationToken);
        }
    }
}
