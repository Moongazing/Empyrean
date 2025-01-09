using AutoMapper;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Constants;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Rules;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Delete;

public class DeleteUserOperationClaimCommand : IRequest<DeleteUserOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, IIntervalRequest, ICacheRemoverRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Write, UserOperationClaimsOperationClaims.Delete, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "UserOperationClaims";
    public string? CacheKey => null;
    public int Interval => 15;

    public class DeleteUserOperationClaimCommandHandler : IRequestHandler<DeleteUserOperationClaimCommand, DeleteUserOperationClaimResponse>
    {
        private readonly IUserOperationClaimRepository userOperationClaimRepository;
        private readonly IMapper mapper;
        private readonly UserOperationClaimBusinessRules userOperationClaimBusinessRules;

        public DeleteUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimRepository,
                                                      IMapper mapper,
                                                      UserOperationClaimBusinessRules userOperationClaimBusinessRules)
        {
            this.userOperationClaimRepository = userOperationClaimRepository;
            this.mapper = mapper;
            this.userOperationClaimBusinessRules = userOperationClaimBusinessRules;
        }

        public async Task<DeleteUserOperationClaimResponse> Handle(DeleteUserOperationClaimCommand request, CancellationToken cancellationToken)
        {
            await userOperationClaimBusinessRules.UserOperationClaimIdShouldExistWhenSelected(request.Id);

            UserOperationClaimEntity mappedUserOperationClaim = mapper.Map<UserOperationClaimEntity>(request);

            UserOperationClaimEntity deletedUserOperationClaim =
                await userOperationClaimRepository.DeleteAsync(mappedUserOperationClaim, true, cancellationToken);

            DeleteUserOperationClaimResponse deletedUserOperationClaimDto =
                mapper.Map<DeleteUserOperationClaimResponse>(deletedUserOperationClaim);

            return deletedUserOperationClaimDto;
        }
    }
}
