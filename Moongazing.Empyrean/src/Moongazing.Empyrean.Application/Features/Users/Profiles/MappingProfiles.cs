using AutoMapper;
using Doing.Retail.Application.Features.Users.Commands.Create;
using Doing.Retail.Application.Features.Users.Commands.Update;
using Doing.Retail.Application.Features.Users.Commands.UpdateFromAuth;
using Doing.Retail.Application.Features.Users.Queries.GetById;
using Doing.Retail.Application.Features.Users.Queries.GetList;
using Doing.Retail.Application.Features.Users.Queries.GetListByDynamic;
using Moongazing.Empyrean.Application.Features.Users.Commands.Create;
using Moongazing.Empyrean.Application.Features.Users.Commands.Delete;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.Users.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {


        CreateMap<CreateUserCommand, UserEntity>().ReverseMap();
        CreateMap<CreateUserResponse, UserEntity>().ReverseMap();

        CreateMap<DeleteUserCommand, UserEntity>().ReverseMap();
        CreateMap<DeleteUserResponse, UserEntity>().ReverseMap();

        CreateMap<UpdateUserCommand, UserEntity>().ReverseMap();
        CreateMap<UpdateUserResponse, UserEntity>().ReverseMap();

        CreateMap<UpdateUserFromAuthCommand, UserEntity>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ReverseMap();

        CreateMap<UpdateUserFromAuthResponse, UserEntity>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();

        CreateMap<UserEntity, GetByIdUserResponse>().ReverseMap();


        CreateMap<UserEntity, GetListUserQuery>().ReverseMap();
        CreateMap<IPagebale<UserEntity>, GetListResponse<GetListUserQuery>>().ReverseMap();

        CreateMap<UserEntity, GetListUserResponse>().ReverseMap();

        CreateMap<IPagebale<UserEntity>, GetListResponse<GetListUserResponse>>().ReverseMap();

        CreateMap<UserEntity, GetListByDynamicUserResponse>()
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();

        CreateMap<IPagebale<UserEntity>, GetListResponse<GetListByDynamicUserResponse>>().ReverseMap();
    }
}
