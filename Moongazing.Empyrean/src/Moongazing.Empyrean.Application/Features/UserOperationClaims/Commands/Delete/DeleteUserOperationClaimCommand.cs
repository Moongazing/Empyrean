using Doing.Retail.Application.Features.UserOperationClaims.Constants;
using Doing.Retail.Application.Features.UserOperationClaims.Rules;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using static Doing.Retail.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;

namespace Doing.Retail.Application.Features.UserOperationClaims.Commands.Delete;

public class DeleteUserOperationClaimCommand : IRequest<DeleteUserOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, IIntervalRequest, ICacheRemoverRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Write, UserOperationClaimsOperationClaims.Delete, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => CacheGroupKeys.UserOperationClaims;
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
