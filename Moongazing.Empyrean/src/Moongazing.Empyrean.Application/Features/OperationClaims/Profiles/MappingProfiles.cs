using AutoMapper;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Delete;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Update;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetById;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetList;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetListByDynamic;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Models;

namespace Doing.Retail.Application.Features.OperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<OperationClaimEntity, CreateOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaimEntity, CreateOperationClaimResponse>().ReverseMap();

        CreateMap<OperationClaimEntity, UpdateOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaimEntity, UpdateOperationClaimResponse>().ReverseMap();

        CreateMap<OperationClaimEntity, DeleteOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaimEntity, DeleteOperationClaimResponse>().ReverseMap();

        CreateMap<OperationClaimEntity, GetByIdOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaimEntity, GetListOperationClaimResponse>().ReverseMap();

        CreateMap<IPagebale<OperationClaimEntity>, GetListResponse<GetListOperationClaimResponse>>().ReverseMap();
        CreateMap<OperationClaimEntity, GetListByDynamicOperationClaimResponse>().ReverseMap();

        CreateMap<IPagebale<OperationClaimEntity>, GetListResponse<GetListByDynamicOperationClaimResponse>>().ReverseMap();
    }
}
