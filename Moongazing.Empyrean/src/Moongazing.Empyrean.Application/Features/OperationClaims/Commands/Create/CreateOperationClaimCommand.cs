using AutoMapper;
using Doing.Retail.Application.Features.OperationClaims.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Doing.Retail.Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;

public class CreateOperationClaimCommand : IRequest<CreateOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, ICacheRemoverRequest, IIntervalRequest, ITransactionalRequest
{
    public string Name { get; set; } = default!;
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "OperationClaims";
    public string? CacheKey => null;
    public int Interval => 15;

    public class CreateOperationClaimCommandHandler : IRequestHandler<CreateOperationClaimCommand, CreateOperationClaimResponse>
    {
        private readonly IOperationClaimRepository operationClaimRepository;
        private readonly IMapper mapper;
        private readonly OperationClaimBusinessRules operationClaimBusinessRules;

        public CreateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository,
                                                  IMapper mapper,
                                                  OperationClaimBusinessRules operationClaimBusinessRules)


        {
            this.operationClaimRepository = operationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<CreateOperationClaimResponse> Handle(CreateOperationClaimCommand request, CancellationToken cancellationToken)
        {
            await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenCreating(request.Name);

            OperationClaimEntity mappedOperationClaim = mapper.Map<OperationClaimEntity>(request);

            OperationClaimEntity createdOperationClaim = await operationClaimRepository.AddAsync(mappedOperationClaim);

            CreateOperationClaimResponse response = mapper.Map<CreateOperationClaimResponse>(createdOperationClaim);

            return response;
        }
    }
}