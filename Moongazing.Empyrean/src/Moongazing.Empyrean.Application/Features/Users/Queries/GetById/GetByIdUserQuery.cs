using AutoMapper;
using Doing.Retail.Application.Features.Users.Rules;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.Users.Constans.UsersOperationClaims;

namespace Doing.Retail.Application.Features.Users.Queries.GetById;

public class GetByIdUserQuery : IRequest<GetByIdUserResponse>,
    ISecuredRequest, ICachableRequest, IIntervalRequest, ILoggableRequest
{
    public Guid Id { get; set; }
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public int Interval => 15;
    public string CacheKey => $"{GetType().Name}({Id})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => "Users";
    public TimeSpan? SlidingExpiration { get; }
    public class GetByIdUserQueryHandler : IRequestHandler<GetByIdUserQuery, GetByIdUserResponse>
    {

        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;
        private readonly UserBusinessRules userBusinessRules;

        public GetByIdUserQueryHandler(IUserRepository userRepository,
                                       IMapper mapper,
                                       UserBusinessRules userBusinessRules)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.userBusinessRules = userBusinessRules;
        }
        public async Task<GetByIdUserResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            UserEntity? user = await userRepository.GetAsync(predicate: b => b.Id == request.Id,
                                                             cancellationToken: cancellationToken);

            await userBusinessRules.UserShouldBeExistsWhenSelected(user!);

            GetByIdUserResponse response = mapper.Map<GetByIdUserResponse>(user!);

            return response;
        }
    }
}
