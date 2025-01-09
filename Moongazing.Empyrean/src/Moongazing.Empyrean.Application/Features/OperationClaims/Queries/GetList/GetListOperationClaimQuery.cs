using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;


namespace Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetList;

public class GetListOperationClaimQuery : IRequest<GetListResponse<GetListOperationClaimResponse>>,
    ISecuredRequest, ILoggableRequest, ICachableRequest, IIntervalRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({PageRequest.PageIndex}-{PageRequest.PageSize})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => "OperationClaims";
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;

    public class GetListOperationClaimQueryHandler
        : IRequestHandler<GetListOperationClaimQuery, GetListResponse<GetListOperationClaimResponse>>
    {
        private readonly IOperationClaimRepository operationClaimRepository;
        private readonly IMapper mapper;

        public GetListOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository,
                                                 IMapper mapper)
        {
            this.operationClaimRepository = operationClaimRepository;
            this.mapper = mapper;
        }

        public async Task<GetListResponse<GetListOperationClaimResponse>> Handle(
            GetListOperationClaimQuery request,
            CancellationToken cancellationToken
        )
        {
            IPagebale<OperationClaimEntity> operationClaims = await operationClaimRepository.GetListAsync(index: request.PageRequest.PageIndex,
                                                                                                          size: request.PageRequest.PageSize,
                                                                                                          cancellationToken: cancellationToken);

            GetListResponse<GetListOperationClaimResponse> response = mapper.Map<GetListResponse<GetListOperationClaimResponse>>(
                operationClaims
            );
            return response;
        }
    }
}