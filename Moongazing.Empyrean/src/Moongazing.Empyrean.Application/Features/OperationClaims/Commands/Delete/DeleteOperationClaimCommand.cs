using AutoMapper;
using Doing.Retail.Application.Features.OperationClaims.Rules;
using MediatR;
using Microsoft.EntityFrameworkCore;
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

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Delete;

public class DeleteOperationClaimCommand : IRequest<DeleteOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, ICacheRemoverRequest, IIntervalRequest, ITransactionalRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Write, OperationClaimsOperationClaims.Delete, GeneralOperationClaims.Write];
    public int Interval => 15;
    public bool BypassCache { get; }
    public string? CacheGroupKey => "OperationClaims";
    public string? CacheKey => null;


    public class DeleteOperationClaimCommandHandler : IRequestHandler<DeleteOperationClaimCommand, DeleteOperationClaimResponse>
    {
        private readonly IOperationClaimRepository operationClaimRepository;
        private readonly IMapper mapper;
        private readonly OperationClaimBusinessRules operationClaimBusinessRules;

        public DeleteOperationClaimCommandHandler(IOperationClaimRepository operationClaimRepository,
                                                  IMapper mapper,
                                                  OperationClaimBusinessRules operationClaimBusinessRules)


        {
            this.operationClaimRepository = operationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<DeleteOperationClaimResponse> Handle(DeleteOperationClaimCommand request, CancellationToken cancellationToken)
        {
            OperationClaimEntity? operationClaim = await operationClaimRepository.GetAsync(
                predicate: oc => oc.Id == request.Id,
                include: q => q.Include(oc => oc.UserOperationClaims),
                cancellationToken: cancellationToken
            );
            await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim!);

            await operationClaimRepository.DeleteAsync(entity: operationClaim!);

            DeleteOperationClaimResponse response = mapper.Map<DeleteOperationClaimResponse>(operationClaim!);
            return response;
        }
    }
}