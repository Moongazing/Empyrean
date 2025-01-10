using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.BankDetails.Constants.BankDetailOperationClaims;

namespace Moongazing.Empyrean.Application.Features.BankDetails.Queries.GetListByDynamic;

public class GetBankDetailListByDynamicQuery : IRequest<GetListResponse<GetBankDetailListByDynamicResponse>>,
 ILoggableRequest, ISecuredRequest, IIntervalRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public DynamicQuery DynamicQuery { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public int Interval => 15;

    public class GetBankDetailListByDynamicQueryHandler : IRequestHandler<GetBankDetailListByDynamicQuery, GetListResponse<GetBankDetailListByDynamicResponse>>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly IMapper mapper;

        public GetBankDetailListByDynamicQueryHandler(IBankDetailRepository bankDetailRepository,
                                                      IMapper mapper)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.mapper = mapper;
        }

        public async Task<GetListResponse<GetBankDetailListByDynamicResponse>> Handle(GetBankDetailListByDynamicQuery request, CancellationToken cancellationToken)
        {
            IPagebale<BankDetailEntity> bookmarkList = await bankDetailRepository.GetListByDynamicAsync(
                dynamic: request.DynamicQuery,
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                include: x => x.Include(x => x.Employee),
                cancellationToken: cancellationToken);

            GetListResponse<GetBankDetailListByDynamicResponse> response = mapper.Map<GetListResponse<GetBankDetailListByDynamicResponse>>(bookmarkList);

            return response;
        }
    }
}
