using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Features.BankDetails.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Moongazing.Empyrean.Application.Features.BankDetails.Constants.BankDetailOperationClaims;
namespace Moongazing.Empyrean.Application.Features.BankDetails.Commands.Delete;

public class DeleteBankDetailCommand : IRequest<DeleteBankDetailResponse>,
ILoggableRequest, ISecuredRequest, ICacheRemoverRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid Id { get; set; } = default!;
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "BankDetails";
    public string? CacheKey => null;
    public int Interval => 15;


    public class DeleteBankDetailCommandHandler : IRequestHandler<DeleteBankDetailCommand, DeleteBankDetailResponse>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly BankDetailBusinessRules bankDetailBusinessRules;
        private readonly IMapper mapper;

        public DeleteBankDetailCommandHandler(IBankDetailRepository bankDetailRepository,
                                              BankDetailBusinessRules bankDetailBusinessRules,
                                              IMapper mapper)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.bankDetailBusinessRules = bankDetailBusinessRules;
            this.mapper = mapper;
        }

        public async Task<DeleteBankDetailResponse> Handle(DeleteBankDetailCommand request, CancellationToken cancellationToken)
        {
            BankDetailEntity? bankDetail = await bankDetailRepository.GetAsync(predicate: e => e.Id == request.Id, withDeleted: false);

            await bankDetailBusinessRules.BankDetailsShouldBeExistsWhenSelected(bankDetail);

            await bankDetailRepository.DeleteAsync(entity: bankDetail!, permanent: true, cancellationToken: cancellationToken);

            DeleteBankDetailResponse response = mapper.Map<DeleteBankDetailResponse>(bankDetail!);

            return response;
        }
    }
}
