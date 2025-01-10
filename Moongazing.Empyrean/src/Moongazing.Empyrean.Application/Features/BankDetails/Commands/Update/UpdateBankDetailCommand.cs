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
namespace Moongazing.Empyrean.Application.Features.BankDetail.Commands.Update;

public class UpdateBankDetailCommand : IRequest<UpdateBankDetailResponse>,
    ILoggableRequest, ISecuredRequest, ICacheRemoverRequest, ITransactionalRequest, IIntervalRequest
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = default!;
    public string AccountNumber { get; set; } = default!;
    public string IBAN { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => BankDetailMessages.SectionName;
    public string? CacheKey => null;
    public int Interval => 15;

    public class UpdateBankDetailCommandHandler : IRequestHandler<UpdateBankDetailCommand, UpdateBankDetailResponse>
    {
        private readonly IBankDetailRepository bankDetailRepository;
        private readonly IMapper mapper;
        private readonly BankDetailBusinessRules bankDetailBusinessRules;
        private readonly EmployeeBusinessRules employeeBusinessRules;

        public UpdateBankDetailCommandHandler(IBankDetailRepository bankDetailRepository,
                                              IMapper mapper,
                                              BankDetailBusinessRules bankDetailBusinessRules,
                                              EmployeeBusinessRules employeeBusinessRules)
        {
            this.bankDetailRepository = bankDetailRepository;
            this.mapper = mapper;
            this.bankDetailBusinessRules = bankDetailBusinessRules;
            this.employeeBusinessRules = employeeBusinessRules;
        }

        public async Task<UpdateBankDetailResponse> Handle(UpdateBankDetailCommand request, CancellationToken cancellationToken)
        {
            BankDetailEntity? existingBankDetail = await bankDetailRepository.GetAsync(
                predicate: x => x.Id == request.Id,
                cancellationToken: cancellationToken);

            await bankDetailBusinessRules.BankDetailsShouldBeExistsWhenSelected(existingBankDetail!);
            await bankDetailBusinessRules.EmployeeShouldOneBankDetail(request.EmployeeId);
            await employeeBusinessRules.EmployeeShouldBeExists(request.EmployeeId);

            var bankDetail = mapper.Map(request, existingBankDetail);

            var result = await bankDetailRepository.UpdateAsync(bankDetail!, cancellationToken);

            UpdateBankDetailResponse response = mapper.Map<UpdateBankDetailResponse>(result);

            return response;
        }
    }
}
