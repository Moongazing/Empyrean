using Doing.Retail.Application.Features.UserOperationClaims.Rules;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using static Doing.Retail.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;

namespace Doing.Retail.Application.Features.UserOperationClaims.Queries.GetById;

public class GetByIdUserOperationClaimQuery : IRequest<GetByIdUserOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, ICachableRequest, IIntervalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Write];
    public int Interval => 15;
    public string CacheKey => $"{GetType().Name}({Id})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => CacheGroupKeys.UserOperationClaims;
    public TimeSpan? SlidingExpiration { get; }


    public class GetByIdUserOperationClaimQueryHandler : IRequestHandler<GetByIdUserOperationClaimQuery, GetByIdUserOperationClaimResponse>
    {

        private readonly IUserOperationClaimRepository userOperationClaimRepository;
        private readonly IMapper mapper;
        private readonly UserOperationClaimBusinessRules operationClaimBusinessRules;

        public GetByIdUserOperationClaimQueryHandler(IUserOperationClaimRepository userOperationClaimRepository,
                                                     IMapper mapper,
                                                     UserOperationClaimBusinessRules operationClaimBusinessRules)
        {
            this.userOperationClaimRepository = userOperationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<GetByIdUserOperationClaimResponse> Handle(GetByIdUserOperationClaimQuery request, CancellationToken cancellationToken)
        {


            UserOperationClaimEntity? userOperationClaim = await userOperationClaimRepository.GetAsync(
                predicate: u => u.Id == request.Id,
                include: u => u.Include(u => u.OperationClaim!),
                cancellationToken: cancellationToken);

            await operationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(userOperationClaim);


            GetByIdUserOperationClaimResponse response = mapper.Map<GetByIdUserOperationClaimResponse>(userOperationClaim!);

            return response;
        }
    }
}
