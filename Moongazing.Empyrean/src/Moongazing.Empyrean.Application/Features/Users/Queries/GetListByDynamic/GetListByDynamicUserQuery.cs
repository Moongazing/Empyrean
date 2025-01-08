using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moongazing.Empyrean.Application.Repositories;
using Moongazing.Kernel.Application.Pipelines.Authorization;
using Moongazing.Kernel.Application.Pipelines.Logging;
using Moongazing.Kernel.Application.Pipelines.Performance;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Constants;
using Moongazing.Kernel.Security.Models;
using static Moongazing.Empyrean.Application.Features.Users.Constans.UsersOperationClaims;


namespace Doing.Retail.Application.Features.Users.Queries.GetListByDynamic;

public class GetListByDynamicUserQuery : IRequest<GetListResponse<GetListByDynamicUserResponse>>,
    ILoggableRequest, ISecuredRequest, IIntervalRequest
{
    public PageRequest PageRequest { get; set; } = default!;
    public DynamicQuery DynamicQuery { get; set; } = default!;
    public string[] Roles => [Admin, Read, GeneralOperationClaims.Read];
    public int Interval => 15;
    public class GetListByDynamicUserQueryHandler : IRequestHandler<GetListByDynamicUserQuery, GetListResponse<GetListByDynamicUserResponse>>
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public GetListByDynamicUserQueryHandler(IUserRepository userRepository,
                                                IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        public async Task<GetListResponse<GetListByDynamicUserResponse>> Handle(GetListByDynamicUserQuery request, CancellationToken cancellationToken)
        {
            IPagebale<UserEntity> users = await userRepository.GetListByDynamicAsync(
                 dynamic: request.DynamicQuery,
                 include: m => m.Include(m => m.UserOperationClaims),
                 index: request.PageRequest.PageSize,
                 size: request.PageRequest.PageSize,
                 cancellationToken: cancellationToken);

            var response = mapper.Map<GetListResponse<GetListByDynamicUserResponse>>(users);

            return response;


        }
    }
}
