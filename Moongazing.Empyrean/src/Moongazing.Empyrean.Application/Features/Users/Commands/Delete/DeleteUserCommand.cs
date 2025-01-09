using AutoMapper;
using Doing.Retail.Application.Features.Users.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
using Moongazing.Empyrean.Application.Features.Users.Constans;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.Users.Constans.UsersOperationClaims;


namespace Moongazing.Empyrean.Application.Features.Users.Commands.Delete;

public class DeleteUserCommand : IRequest<DeleteUserResponse>,
    ISecuredRequest, ILoggableRequest, IIntervalRequest, ICacheRemoverRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Write, UsersOperationClaims.Delete, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "Users";
    public string? CacheKey => null;
    public int Interval => 15;

    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, DeleteUserResponse>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly UserBusinessRules userBusinessRules;

        public DeleteUserCommandHandler(IUserRepository userRepository,
                                        IMapper mapper,
                                        UserBusinessRules userBusinessRules)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userBusinessRules = userBusinessRules;
        }

        public async Task<DeleteUserResponse> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            UserEntity? user = await userRepository.GetAsync(predicate: u => u.Id == request.Id,
                                                             cancellationToken: cancellationToken);

            await userBusinessRules.UserShouldBeExistsWhenSelected(user!);

            await userRepository.DeleteAsync(user!, true, cancellationToken);

            DeleteUserResponse response = mapper.Map<DeleteUserResponse>(user!);

            return response;
        }
    }
}

