using AutoMapper;
using Doing.Retail.Application.Features.Users.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Features.Users.Constans;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Hashing;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.Users.Constans.UsersOperationClaims;

namespace Doing.Retail.Application.Features.Users.Commands.Update;

public class UpdateUserCommand : IRequest<UpdateUserResponse>,
    ISecuredRequest, ILoggableRequest, IIntervalRequest, ICacheRemoverRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Cd_Operatore { get; set; } = default!;
    public string[] Roles => [Admin, Write, UsersOperationClaims.Update, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "Users";
    public string? CacheKey => null;
    public int Interval => 15;

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UpdateUserResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly UserBusinessRules userBusinessRules;

        public UpdateUserCommandHandler(IUserRepository userRepository,
                                        IMapper mapper,
                                        UserBusinessRules userBusinessRules)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userBusinessRules = userBusinessRules;
        }

        public async Task<UpdateUserResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            UserEntity? existingUser = await userRepository.GetAsync(predicate: u => u.Id == request.Id,
                                                                     cancellationToken: cancellationToken);


            await userBusinessRules.UserShouldBeExistsWhenSelected(existingUser!);

            await userBusinessRules.UserEmailShouldNotExistsWhenUpdate(request.Email);

            var updatedUser = mapper.Map(request, existingUser);

            HashingHelper.CreateHash(
                request.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt
            );
            updatedUser!.PasswordHash = passwordHash;
            updatedUser.PasswordSalt = passwordSalt;

            await userRepository.UpdateAsync(updatedUser!, cancellationToken);

            UpdateUserResponse response = mapper.Map<UpdateUserResponse>(updatedUser);
            return response;
        }
    }
}
