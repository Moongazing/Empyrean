﻿using Doing.Retail.Application.Features.UserOperationClaims.Rules;
using Doing.Retail.Application.Services.Repositories;
using MediatR;
using static Doing.Retail.Application.Features.UserOperationClaims.Constants.UserOperationClaimsOperationClaims;


namespace Doing.Retail.Application.Features.UserOperationClaims.Commands.Create;

public class CreateUserOperationClaimCommand : IRequest<CreateUserOperationClaimResponse>,
    ISecuredRequest, ILoggableRequest, ITransactionalRequest, ICacheRemoverRequest, IIntervalRequest
{
    public Guid UserId { get; set; }
    public Guid OperationClaimId { get; set; }
    public string[] Roles => [Admin, Write, Add, GeneralOperationClaims.Write];
    public bool BypassCache { get; }
    public string? CacheGroupKey => CacheGroupKeys.UserOperationClaims;
    public string? CacheKey => null;
    public int Interval => 15;

    public class CreateUserOperationClaimCommandHandler : IRequestHandler<CreateUserOperationClaimCommand, CreateUserOperationClaimResponse>
    {
        private readonly IUserOperationClaimRepository userOperationClaimRepository;
        private readonly IMapper mapper;
        private readonly UserOperationClaimBusinessRules operationClaimBusinessRules;

        public CreateUserOperationClaimCommandHandler(IUserOperationClaimRepository userOperationClaimRepository,
                                                      IMapper mapper,
                                                      UserOperationClaimBusinessRules operationClaimBusinessRules)
        {
            this.userOperationClaimRepository = userOperationClaimRepository;
            this.mapper = mapper;
            this.operationClaimBusinessRules = operationClaimBusinessRules;
        }

        public async Task<CreateUserOperationClaimResponse> Handle(CreateUserOperationClaimCommand request, CancellationToken cancellationToken)
        {

            UserOperationClaimEntity mappedUserOperationClaim = mapper.Map<UserOperationClaimEntity>(request);

            await operationClaimBusinessRules.UserShouldNotHaveOperationClaimAlreadyWhenInsert(request.UserId, request.OperationClaimId);

            UserOperationClaimEntity createdUserOperationClaim = await userOperationClaimRepository.AddAsync(mappedUserOperationClaim, cancellationToken);

            CreateUserOperationClaimResponse createdUserOperationClaimDto = mapper.Map<CreateUserOperationClaimResponse>(createdUserOperationClaim);

            return createdUserOperationClaimDto;
        }
    }
}
