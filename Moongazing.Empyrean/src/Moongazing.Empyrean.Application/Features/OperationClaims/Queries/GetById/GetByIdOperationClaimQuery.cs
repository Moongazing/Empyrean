using AutoMapper;
using Doing.Retail.Application.Features.OperationClaims.Rules;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Doing.Retail.Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;


namespace Doing.Retail.Application.Features.OperationClaims.Queries.GetById;

public class GetByIdOperationClaimQuery : IRequest<GetByIdOperationClaimResponse>,
    ISecuredRequest, IIntervalRequest, ICachableRequest, ILoggableRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({Id})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => "OperationClaims";
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;
    public class GetByIdOperationClaimQueryHandler : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimResponse>
    {
        private readonly IOperationClaimRepository operationClaimRepository;
        private readonly IMapper mapper;
        private readonly OperationClaimBusinessRules operationClaimBusinessRules;

        public GetByIdOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository,
                                                 IMapper mapper,
                                                 OperationClaimBusinessRules operationClaimBusinessRules)
        {
            this.operationClaimRepository = operationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<GetByIdOperationClaimResponse> Handle(GetByIdOperationClaimQuery request, CancellationToken cancellationToken)
        {
            OperationClaimEntity? operationClaim = await operationClaimRepository.GetAsync(
                predicate: b => b.Id == request.Id,
                include: q => q.Include(oc => oc.UserOperationClaims),
                cancellationToken: cancellationToken
            );
            await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim!);

            GetByIdOperationClaimResponse response = mapper.Map<GetByIdOperationClaimResponse>(operationClaim!);
            return response;
        }
    }
}