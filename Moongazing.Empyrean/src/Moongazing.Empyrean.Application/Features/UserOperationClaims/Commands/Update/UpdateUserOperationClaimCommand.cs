using Doing.Retail.Application.Features.UserOperationClaims.Constants;
using Doing.Retail.Application.Features.UserOperationClaims.Rules;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using static Doing.Retail.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;

namespace Doing.Retail.Application.Features.UserOperationClaims.Commands.Update;

public class UpdateUserOperationClaimCommand : IRequest<UpdateUserOperationClaimResponse>, ISecuredRequest, ILoggableRequest, ITransactionalRequest, ICacheRemoverRequest, IIntervalRequest
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }
    public string[] Roles => [Admin, Write, UserOperationClaimsOperationClaims.Update, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => CacheGroupKeys.UserOperationClaims;
    public string? CacheKey => null;
    public int Interval => 15;

    public class UpdateUserOperationClaimCommandHandler : IRequestHandler<UpdateUserOperationClaimCommand, UpdateUserOperationClaimResponse>
    {
        private readonly IUserOperationClaimRepository userOperationClaimRepository;
        private readonly IMapper mapper;
        private readonly UserOperationClaimBusinessRules operationClaimBusinessRules;

        public UpdateUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimRepository,
                                                      IMapper mapper,
                                                      UserOperationClaimBusinessRules operationClaimBusinessRules)
        {
            this.userOperationClaimRepository = userOperationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<UpdateUserOperationClaimResponse> Handle(UpdateUserOperationClaimCommand request, CancellationToken cancellationToken)
        {
            UserOperationClaimEntity? existingUserOperationClaim = await userOperationClaimRepository.GetAsync(
                x => x.Id == request.Id,
                cancellationToken: cancellationToken);

            await operationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(existingUserOperationClaim!);

            var updatedUserOperationClaim = mapper.Map(request, existingUserOperationClaim);

            await userOperationClaimRepository.UpdateAsync(existingUserOperationClaim!,
                                                           updatedUserOperationClaim!,
                                                           cancellationToken);

            UpdateUserOperationClaimResponse response = mapper.Map<UpdateUserOperationClaimResponse>(updatedUserOperationClaim!);

            return response;
        }
    }
}
