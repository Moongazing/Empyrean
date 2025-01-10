using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using static Moongazing.Empyrean.Application.Features.BankDetails.Constants.BankDetailOperationClaims;

namespace Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetList;

public class GetBankDetailListQuery : IRequest<GetListResponse<GetBankDetailListResponse>>,
    ILoggableRequest, ICachableRequest, ISecuredRequest, IIntervalRequest

{
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({PageRequest.PageIndex}-{PageRequest.PageSize})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => BankDetailMessages.SectionName;
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;

    public class GetBankDetailListQueryHandler : IRequestHandler<GetBankDetailListQuery, GetListResponse<GetBankDetailListResponse>>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly IMapper mapper;

        public GetBankDetailListQueryHandler(IBankDetailRepository bankDetailRepository,
                                             IMapper mapper)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.mapper = mapper;
        }

        public async Task<GetListResponse<GetBankDetailListResponse>> Handle(GetBankDetailListQuery request, CancellationToken cancellationToken)
        {
            IPagebale<BankDetailEntity> bankDetailList = await bankDetailRepository.GetListAsync(
               index: request.PageRequest.PageIndex,
               size: request.PageRequest.PageSize,
               include: x => x.Include(x => x.Employee),
               cancellationToken: cancellationToken);

            GetListResponse<GetBankDetailListResponse> response = mapper.Map<GetListResponse<GetBankDetailListResponse>>(bankDetailList);

            return response;
        }
    }
}
