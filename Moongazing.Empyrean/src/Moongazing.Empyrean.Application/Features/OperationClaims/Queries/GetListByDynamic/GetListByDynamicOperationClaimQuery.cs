using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;


namespace Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetListByDynamic;

public class GetListByDynamicOperationClaimQuery : IRequest<GetListResponse<GetListByDynamicOperationClaimResponse>>, ILoggableRequest, ISecuredRequest, IIntervalRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public DynamicQuery DynamicQuery { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public int Interval => 15;

    public class GetListByDynamicOperationClaimQueryHandler : IRequestHandler<GetListByDynamicOperationClaimQuery, GetListResponse<GetListByDynamicOperationClaimResponse>>
    {
        private readonly IOperationClaimRepository operationClaimRepository;
        private readonly IMapper mapper;
        public GetListByDynamicOperationClaimQueryHandler(IOperationClaimRepository operationClaimRepository,
                                                          IMapper mapper)
        {
            this.operationClaimRepository = operationClaimRepository;
            this.mapper = mapper;
        }
        public async Task<GetListResponse<GetListByDynamicOperationClaimResponse>> Handle(GetListByDynamicOperationClaimQuery request, CancellationToken cancellationToken)
        {
            IPagebale<OperationClaimEntity> opertaionClaim = await operationClaimRepository.GetListByDynamicAsync(dynamic: request.DynamicQuery,
                                                                                                                  index: request.PageRequest.PageIndex,
                                                                                                                  size: request.PageRequest.PageSize,
                                                                                                                  cancellationToken: cancellationToken);


            GetListResponse<GetListByDynamicOperationClaimResponse> response = mapper.Map<GetListResponse<GetListByDynamicOperationClaimResponse>>(opertaionClaim);

            return response;
        }
    }

}

