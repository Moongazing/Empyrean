using Doing.Retail.Application.Features.Authentication.Rules;
using Doing.Retail.Application.Services.User;
using MediatR;
using MimeKit;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Application.Services.AuthenticatorService;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Mailing;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;
using System.Web;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.EnableEmailAuthenticator;

public class EnableEmailAuthenticatorCommand : IRequest, ILoggableRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid UserId { get; set; } = default!;
    public string VerifyEmailUrlPrefix { get; set; } = default!;
    public int Interval => 15;


    public class EnableEmailAuthenticatorCommandHandler : IRequestHandler<EnableEmailAuthenticatorCommand>
    {
        private readonly AuthBusinessRules authBusinessRules;
        private readonly IAuthenticatorService authenticatorService;
        private readonly IEmailAuthenticatorRepository emailAuthenticatorRepository;
        private readonly IMailService mailService;
        private readonly IUserService userService;
        private readonly IUserRepository userRepository;

        public EnableEmailAuthenticatorCommandHandler(AuthBusinessRules authBusinessRules,
                                                      IAuthenticatorService authenticatorService,
                                                      IEmailAuthenticatorRepository emailAuthenticatorRepository,
                                                      IMailService mailService,
                                                      IUserService userService,
                                                      IUserRepository userRepository)
        {
            this.authBusinessRules = authBusinessRules;
            this.authenticatorService = authenticatorService;
            this.emailAuthenticatorRepository = emailAuthenticatorRepository;
            this.mailService = mailService;
            this.userService = userService;
            this.userRepository = userRepository;
        }

        public async Task Handle(EnableEmailAuthenticatorCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await userService.GetAsync(predicate: u => u.Id == request.UserId,
                                                          cancellationToken: cancellationToken);

            await authBusinessRules.UserShouldBeExistsWhenSelected(user);
            await authBusinessRules.UserShouldNotBeHaveAuthenticator(user!);

            var updatedUser = user!;
            updatedUser.AuthenticatorType = AuthenticatorType.Email;

            await userRepository.UpdateAsync(updatedUser, cancellationToken);

            EmailAuthenticatorEntity emailAuthenticator = await authenticatorService.CreateEmailAuthenticator(user!);
            EmailAuthenticatorEntity addedEmailAuthenticator =
                await emailAuthenticatorRepository.AddAsync(emailAuthenticator, cancellationToken);

            var toEmailList = new List<MailboxAddress> { new(name: $"{user!.FirstName} {user.LastName}", user.Email) };

            mailService.SendMail(
               new Mail(
                   subject: "Verify Your Email - Empyrean",
                   textBody: $"Click on the link to verify your email: {request.VerifyEmailUrlPrefix}?ActivationKey={HttpUtility.UrlEncode(addedEmailAuthenticator.ActivationKey)}",
                   htmlBody: string.Empty,
                   toList: toEmailList
               )
            );
        }
    }
}
