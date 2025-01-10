using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetByEmployeeId;
using Moongazing.Empyrean.Application.Features.BankDetails.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.BankDetails.Constants.BankDetailOperationClaims;

namespace Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetById;

public class GetBankDetailByIdQuery : IRequest<GetBankDetailByIdResponse>,
ILoggableRequest, ICachableRequest, ISecuredRequest, IIntervalRequest

{
    public Guid Id { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({Id})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => "BankDetails";
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;


    public class GetBankDetailByIdQueryHandler : IRequestHandler<GetBankDetailByIdQuery, GetBankDetailByIdResponse>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly IMapper mapper;
        private readonly BankDetailBusinessRules bankDetailBusinessRules;

        public GetBankDetailByIdQueryHandler(IBankDetailRepository bankDetailRepository,
                                             IMapper mapper,
                                             BankDetailBusinessRules bankDetailBusinessRules)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.mapper = mapper;
            this.bankDetailBusinessRules = bankDetailBusinessRules;
        }

        public async Task<GetBankDetailByIdResponse> Handle(GetBankDetailByIdQuery request, CancellationToken cancellationToken)
        {
            BankDetailEntity? bankDetail = await bankDetailRepository.GetAsync(
               predicate: b => b.Id == request.Id,
               include: x => x.Include(x => x.Employee),
               cancellationToken: cancellationToken);

            await bankDetailBusinessRules.BankDetailsShouldBeExistsWhenSelected(bankDetail!);

            GetBankDetailByIdResponse response = mapper.Map<GetBankDetailByIdResponse>(bankDetail!);

            return response;
        }
    }
}
