using AutoMapper;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Create;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Delete;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Update;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetById;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetList;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetListDynamic;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.UserOperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserOperationClaimEntity, CreateUserOperationClaimCommand>();
        CreateMap<UserOperationClaimEntity, CreateUserOperationClaimResponse>();

        CreateMap<UserOperationClaimEntity, UpdateUserOperationClaimCommand>();
        CreateMap<UserOperationClaimEntity, UpdateUserOperationClaimResponse>();

        CreateMap<UserOperationClaimEntity, DeleteUserOperationClaimCommand>();
        CreateMap<UserOperationClaimEntity, DeleteUserOperationClaimResponse>();

        CreateMap<UserOperationClaimEntity, GetByIdUserOperationClaimQuery>();
        CreateMap<UserOperationClaimEntity, GetByIdUserOperationClaimResponse>()
            .ForMember(dest => dest.OperationClaimName, opt => opt.MapFrom(src => src.OperationClaim.Name))
            .ReverseMap();

        CreateMap<UserOperationClaimEntity, GetListUserOperationClaimResponse>()
            .ForMember(dest => dest.OperationClaimName, opt => opt.MapFrom(src => src.OperationClaim.Name));

        CreateMap<IPagebale<UserOperationClaimEntity>, GetListResponse<GetListUserOperationClaimResponse>>();

        CreateMap<UserOperationClaimEntity, GetListByDynamicUserOperationClaimResponse>()
            .ForMember(dest => dest.OperationClaimName, opt => opt.MapFrom(src => src.OperationClaim.Name))
            .ReverseMap();

        CreateMap<IPagebale<UserOperationClaimEntity>, GetListResponse<GetListByDynamicUserOperationClaimResponse>>()
            .ReverseMap();
    }
}
