﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
using Moongazing.Empyrean.Application.Features.BankDetails.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Security.Constants;
using static Moongazing.Empyrean.Application.Features.BankDetails.Constants.BankDetailOperationClaims;

namespace Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetByEmployeeId;

public class GetBankDetailByEmployeeIdQuery : IRequest<GetBankDetailByEmployeeIdResponse>,
        ILoggableRequest, ICachableRequest, ISecuredRequest, IIntervalRequest
{
    public Guid EmployeeId { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public string CacheKey => $"{GetType().Name}({EmployeeId})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => BankDetailMessages.SectionName;
    public TimeSpan? SlidingExpiration { get; }
    public int Interval => 15;


    public class GetBankDetailByEmployeeQueryHandler : IRequestHandler<GetBankDetailByEmployeeIdQuery, GetBankDetailByEmployeeIdResponse>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly IMapper mapper;
        private readonly BankDetailBusinessRules bankDetailBusinessRules;

        public GetBankDetailByEmployeeQueryHandler(IBankDetailRepository bankDetailRepository,
                                                   IMapper mapper,
                                                   BankDetailBusinessRules bankDetailBusinessRules)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.mapper = mapper;
            this.bankDetailBusinessRules = bankDetailBusinessRules;
        }

        public async Task<GetBankDetailByEmployeeIdResponse> Handle(GetBankDetailByEmployeeIdQuery request, CancellationToken cancellationToken)
        {
            BankDetailEntity? bankDetail = await bankDetailRepository.GetAsync(
                predicate: b => b.EmployeeId == request.EmployeeId,
                include: x => x.Include(x => x.Employee),
                cancellationToken: cancellationToken);

            await bankDetailBusinessRules.BankDetailsShouldBeExistsWhenSelected(bankDetail!);

            GetBankDetailByEmployeeIdResponse response = mapper.Map<GetBankDetailByEmployeeIdResponse>(bankDetail!);

            return response;
        }
    }
}
