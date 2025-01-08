using AutoMapper;
using Doing.Retail.Application.Features.Users.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Application.Services.Auth;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Hashing;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.Users.Commands.UpdateFromAuth;

public class UpdateUserFromAuthCommand : IRequest<UpdateUserFromAuthResponse>,
    ILoggableRequest, ITransactionalRequest, IIntervalRequest, ICacheRemoverRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? NewPassword { get; set; }
    public int Interval => 15;
    public bool BypassCache { get; }
    public string? CacheGroupKey => "Users";
    public string? CacheKey => null;


    public class UpdateUserFromAuthCommandHandler : IRequestHandler<UpdateUserFromAuthCommand, UpdateUserFromAuthResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly UserBusinessRules userBusinessRules;
        private readonly IAuthService authService;

        public UpdateUserFromAuthCommandHandler(IUserRepository userRepository,
                                                IMapper mapper,
                                                UserBusinessRules userBusinessRules,
                                                IAuthService authService)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userBusinessRules = userBusinessRules;
            this.authService = authService;
        }

        public async Task<UpdateUserFromAuthResponse> Handle(UpdateUserFromAuthCommand request, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await userRepository.GetAsync(predicate: u => u.Id == request.Id,
                                                                     cancellationToken: cancellationToken);

            await userBusinessRules.UserShouldBeExistsWhenSelected(existingUser!);

            await userBusinessRules.UserPasswordShouldBeMatched(user: existingUser!, request.Password);

            await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(existingUser!.Email);

            var updatedUser = mapper.Map(request, existingUser);

            if (request.NewPassword != null && !string.IsNullOrWhiteSpace(request.NewPassword))
            {
                HashingHelper.CreateHash(
                    request.NewPassword,
                    out byte[] passwordHash,
                    out byte[] passwordSalt
                );
                updatedUser.PasswordHash = passwordHash;
                updatedUser.PasswordSalt = passwordSalt;
            }

            await userRepository.UpdateAsync(updatedUser!, cancellationToken);

            UpdateUserFromAuthResponse response = mapper.Map<UpdateUserFromAuthResponse>(updatedUser);
            response.AccessToken = await authService.CreateAccessToken(updatedUser);
            return response;
        }
    }
}
