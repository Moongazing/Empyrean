using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Features.BankDetails.Constants;
using Moongazing.Empyrean.Application.Features.BankDetails.Rules;
using Moongazing.Empyrean.Application.Features.Employee.Rules;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using static Moongazing.Empyrean.Application.Features.BankDetails.Constants.BankDetailOperationClaims;

namespace Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;

public class CreateBankDetailCommand : IRequest<CreateBankDetailResponse>,
ILoggableRequest, ISecuredRequest, ICacheRemoverRequest, ITransactionalRequest, IIntervalRequest
{
    public string BankName { get; set; } = default!;
    public string AccountNumber { get; set; } = default!;
    public string IBAN { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => BankDetailMessages.SectionName;
    public string? CacheKey => null;
    public int Interval => 15;



    public class CreateBankDetailCommandHandler : IRequestHandler<CreateBankDetailCommand, CreateBankDetailResponse>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly BankDetailBusinessRules bankDetailBusinessRules;
        private readonly EmployeeBusinessRules employeeBusinessRules;
        private readonly IMapper mapper;

        public CreateBankDetailCommandHandler(IBankDetailRepository bankDetailRepository,
                                              BankDetailBusinessRules bankDetailBusinessRules,
                                              EmployeeBusinessRules employeeBusinessRules,
                                              IMapper mapper)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.bankDetailBusinessRules = bankDetailBusinessRules;
            this.employeeBusinessRules = employeeBusinessRules;
            this.mapper = mapper;
        }

        public async Task<CreateBankDetailResponse> Handle(CreateBankDetailCommand request, CancellationToken cancellationToken)
        {
            await employeeBusinessRules.EmployeeShouldBeExists(employeeId: request.EmployeeId);
            await bankDetailBusinessRules.BankDetailsShouldBeUnique(iban: request.IBAN, accountNumber: request.AccountNumber);
            await bankDetailBusinessRules.EmployeeShouldOneBankDetail(employeeId: request.EmployeeId);

            BankDetailEntity? bankDetail = mapper.Map<BankDetailEntity>(request);

            BankDetailEntity addedBankDetail = await bankDetailRepository.AddAsync(bankDetail);

            CreateBankDetailResponse response = mapper.Map<CreateBankDetailResponse>(addedBankDetail);

            return response;




        }
    }
}
