using AutoMapper;
using Moongazing.Empyrean.Application.Features.BankDetail.Commands.Update;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetByEmployeeId;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetById;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetList;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Delete;
using Moongazing.Empyrean.Application.Features.BankDetails.Queries.GetListByDynamic;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Delete;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Update;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetById;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetList;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetListByDynamic;
using Moongazing.Empyrean.Domain.Entities;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Paging;
using Moongazing.Kernel.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moongazing.Empyrean.Application.Features.BankDetails.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<BankDetailEntity, CreateBankDetailCommand>().ReverseMap();
        CreateMap<BankDetailEntity, CreateBankDetailResponse>().ReverseMap();

        CreateMap<BankDetailEntity, UpdateBankDetailCommand>().ReverseMap();
        CreateMap<BankDetailEntity, UpdateBankDetailResponse>().ReverseMap();

        CreateMap<BankDetailEntity, DeleteBankDetailCommand>().ReverseMap();
        CreateMap<BankDetailEntity, DeleteBankDetailResponse>().ReverseMap();

        CreateMap<BankDetailEntity, GetBankDetailByIdQuery>().ReverseMap();
        CreateMap<BankDetailEntity, GetBankDetailByIdResponse>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id))
            .ReverseMap();

        CreateMap<BankDetailEntity, GetBankDetailByEmployeeIdQuery>().ReverseMap();
        CreateMap<BankDetailEntity, GetBankDetailByEmployeeIdResponse>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id))
            .ReverseMap();


        CreateMap<BankDetailEntity, GetBankDetailListResponse>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id))
            .ReverseMap();
        CreateMap<IPagebale<BankDetailEntity>, GetListResponse<GetBankDetailListResponse>>().ReverseMap();


        CreateMap<IPagebale<BankDetailEntity>, GetListResponse<GetBankDetailListByDynamicResponse>>().ReverseMap();
        CreateMap<BankDetailEntity, GetBankDetailListByDynamicResponse>()
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.Employee.Id))
            .ReverseMap();
    }
}
