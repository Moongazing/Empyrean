using AutoMapper;
using Doing.Retail.Application.Features.OperationClaims.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Features.OperationClaims.Constants;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Pipelines.Transaction;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.OperationClaims.Constants.OperationClaimsOperationClaims;

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Update;

public class UpdateOperationClaimCommand : IRequest<UpdateOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, ICacheRemoverRequest, IIntervalRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string[] Roles => [Admin, Write, OperationClaimsOperationClaims.Update, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => "OperationClaims";
    public string? CacheKey => null;
    public int Interval => 15;
    public class UpdateOperationClaimCommandHandler : IRequestHandler<UpdateOperationClaimCommand, UpdateOperationClaimResponse>
    {
        private readonly IOperationClaimRepository operationClaimRepository;
        private readonly IMapper mapper;
        private readonly OperationClaimBusinessRules operationClaimBusinessRules;

        public UpdateOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository,
                                                  IMapper mapper,
                                                  OperationClaimBusinessRules operationClaimBusinessRules)


        {
            this.operationClaimRepository = operationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<UpdateOperationClaimResponse> Handle(UpdateOperationClaimCommand request, CancellationToken cancellationToken)
        {
            OperationClaimEntity? operationClaim = await operationClaimRepository.GetAsync(
                predicate: oc => oc.Id == request.Id,
                cancellationToken: cancellationToken
            );
            await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);

            await operationClaimBusinessRules.OperationClaimNameShouldNotExistWhenUpdating(request.Id, request.Name);

            OperationClaimEntity mappedOperationClaim = mapper.Map(request, destination: operationClaim!);

            OperationClaimEntity updatedOperationClaim = await operationClaimRepository.UpdateAsync(mappedOperationClaim);

            UpdateOperationClaimResponse response = mapper.Map<UpdateOperationClaimResponse>(updatedOperationClaim);
            return response;
        }
    }
}
