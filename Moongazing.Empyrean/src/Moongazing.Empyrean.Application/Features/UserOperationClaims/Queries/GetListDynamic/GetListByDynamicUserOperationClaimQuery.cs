using AutoMapper;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;


namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetListDynamic;

public class GetListByDynamicUserOperationClaimQuery : IRequest<GetListResponse<GetListByDynamicUserOperationClaimResponse>>,
    ISecuredRequest, ILoggableRequest, IIntervalRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public DynamicQuery DynamicQuery { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Write];
    public int Interval => 15;


    public class GetListByDynamicUserOperationClaimQueryHandler : IRequestHandler<GetListByDynamicUserOperationClaimQuery, GetListResponse<GetListByDynamicUserOperationClaimResponse>>
    {
        private readonly IUserOperationClaimRepository userOperationClaimRepository;
        private readonly IMapper mapper;

        public GetListByDynamicUserOperationClaimQueryHandler(IUserOperationClaimRepository userOperationClaimRepository,
                                                              IMapper mapper)
        {
            this.userOperationClaimRepository = userOperationClaimRepository;
            this.mapper = mapper;
        }
        public async Task<GetListResponse<GetListByDynamicUserOperationClaimResponse>> Handle(GetListByDynamicUserOperationClaimQuery request, CancellationToken cancellationToken)
        {
            IPagebale<UserOperationClaimEntity> userOperationClaims = await userOperationClaimRepository.GetListByDynamicAsync(
                 dynamic: request.DynamicQuery,
                 index: request.PageRequest.PageIndex,
                 size: request.PageRequest.PageSize,
                 include: m => m.Include(m => m.OperationClaim),
                 cancellationToken: cancellationToken);

            var response = mapper.Map<GetListResponse<GetListByDynamicUserOperationClaimResponse>>(userOperationClaims);

            return response;


        }
    }
}
