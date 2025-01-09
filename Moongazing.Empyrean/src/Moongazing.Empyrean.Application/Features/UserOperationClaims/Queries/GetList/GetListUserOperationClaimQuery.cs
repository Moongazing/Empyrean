using AutoMapper;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;


namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetList;

public class GetListUserOperationClaimQuery : IRequest<GetListResponse<GetListUserOperationClaimResponse>>,
ISecuredRequest, IIntervalRequest, ILoggableRequest, ICachableRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Write];
    public int Interval => 15;
    public string CacheKey => $"{GetType().Name}({PageRequest.PageIndex}-{PageRequest.PageSize})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => "UserOperationClaims";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListUserOperationClaimQueryHandler : IRequestHandler<GetListUserOperationClaimQuery, GetListResponse<GetListUserOperationClaimResponse>>

    {
        private readonly IUserOperationClaimRepository userOperationClaimRepository;
        private readonly IMapper mapper;

        public GetListUserOperationClaimQueryHandler(IUserOperationClaimRepository userOperationClaimRepository,
                                                    IMapper mapper)
        {
            this.userOperationClaimRepository = userOperationClaimRepository;
            this.mapper = mapper;
        }

        public async Task<GetListResponse<GetListUserOperationClaimResponse>> Handle(GetListUserOperationClaimQuery request, CancellationToken cancellationToken)
        {

            IPagebale<UserOperationClaimEntity> userOperationClaims = await userOperationClaimRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                include: u => u.Include(u => u.OperationClaim),
                cancellationToken: cancellationToken);

            var mappedUserOperationClaimListModel = mapper.Map<GetListResponse<GetListUserOperationClaimResponse>>(userOperationClaims);

            return mappedUserOperationClaimListModel;
        }
    }
}
