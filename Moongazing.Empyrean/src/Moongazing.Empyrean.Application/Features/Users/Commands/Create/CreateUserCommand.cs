using AutoMapper;
using Doing.Retail.Application.Features.Users.Commands.Create;
using Doing.Retail.Application.Features.Users.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Hashing;
using Moongazing.Kernel.Security.Models;
using Moongazing.Kernel.Security.Models.Enums;
using static Moongazing.Empyrean.Application.Features.Users.Constans.UsersOperationClaims;


namespace Moongazing.Empyrean.Application.Features.Users.Commands.Create;

public class CreateUserCommand : IRequest<CreateUserResponse>,
    ISecuredRequest, ILoggableRequest, IIntervalRequest, ITransactionalRequest, ICacheRemoverRequest
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public UserStatus Status { get; set; } = default!;
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "Users";
    public string? CacheKey => null;
    public int Interval => 15;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, CreateUserResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly UserBusinessRules userBusinessRules;

        public CreateUserCommandHandler(IUserRepository userRepository,
                                        IMapper mapper,
                                        UserBusinessRules userBusinessRules)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userBusinessRules = userBusinessRules;
        }

        public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await userBusinessRules.UserEmailShouldNotExistsWhenInsert(request.Email);


            UserEntity user = mapper.Map<UserEntity>(request);

            HashingHelper.CreateHash(
                request.Password,
                inputHash: out byte[] passwordHash,
                inputSalt: out byte[] passwordSalt
            );
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            UserEntity createdUser = await userRepository.AddAsync(user, cancellationToken);

            CreateUserResponse response = mapper.Map<CreateUserResponse>(createdUser);
            return response;
        }

    }
}

