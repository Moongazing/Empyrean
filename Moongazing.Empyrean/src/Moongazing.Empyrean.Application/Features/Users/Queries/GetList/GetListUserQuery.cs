using AutoMapper;
using MediatR;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Caching;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.Users.Constans.UsersOperationClaims;

namespace Doing.Retail.Application.Features.Users.Queries.GetList;

public class GetListUserQuery : IRequest<GetListResponse<GetListUserResponse>>,
    ISecuredRequest, ICachableRequest, ILoggableRequest, IIntervalRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public int Interval => 15;
    public string CacheKey => $"GetType().Name({PageRequest.PageIndex}-{PageRequest.PageSize})";
    public bool BypassCache { get; }
    public string? CacheGroupKey => "Users";
    public TimeSpan? SlidingExpiration { get; }

    public class GetListUserQueryHandler : IRequestHandler<GetListUserQuery, GetListResponse<GetListUserResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetListUserQueryHandler(IUserRepository userRepository,
                                       IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<GetListResponse<GetListUserResponse>> Handle(GetListUserQuery request, CancellationToken cancellationToken)
        {

            IPagebale<UserEntity> users = await userRepository.GetListAsync(
                index: request.PageRequest.PageIndex,
                size: request.PageRequest.PageSize,
                cancellationToken: cancellationToken);

            GetListResponse<GetListUserResponse> response = mapper.Map<GetListResponse<GetListUserResponse>>(users);

            return response;
        }
    }

}

