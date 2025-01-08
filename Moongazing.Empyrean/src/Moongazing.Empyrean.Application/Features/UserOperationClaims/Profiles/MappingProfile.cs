using Doing.Retail.Application.Features.UserOperationClaims.Commands.Create;
using Doing.Retail.Application.Features.UserOperationClaims.Commands.Delete;
using Doing.Retail.Application.Features.UserOperationClaims.Commands.Update;
using Doing.Retail.Application.Features.UserOperationClaims.Queries.GetById;
using Doing.Retail.Application.Features.UserOperationClaims.Queries.GetList;
using Doing.Retail.Application.Features.UserOperationClaims.Queries.GetListDynamic;

namespace Doing.Retail.Application.Features.UserOperationClaims.Profiles;

public class MappingProfiles : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserOperationClaimEntity, CreateUserOperationClaimCommand>();
        config.NewConfig<UserOperationClaimEntity, CreateUserOperationClaimResponse>();

        config.NewConfig<UserOperationClaimEntity, UpdateUserOperationClaimCommand>();
        config.NewConfig<UserOperationClaimEntity, UpdateUserOperationClaimResponse>();

        config.NewConfig<UserOperationClaimEntity, DeleteUserOperationClaimCommand>();
        config.NewConfig<UserOperationClaimEntity, DeleteUserOperationClaimResponse>();

        config.NewConfig<UserOperationClaimEntity, GetByIdUserOperationClaimQuery>();
        config.NewConfig<UserOperationClaimEntity, GetByIdUserOperationClaimResponse>()
              .Map(dest => dest.OperationClaimName, src => src.OperationClaim.Name)
              .TwoWays();

        config.NewConfig<UserOperationClaimEntity, GetListUserOperationClaimResponse>()
              .Map(dest => dest.OperationClaimName, src => src.OperationClaim.Name);

        config.NewConfig<Paginate<UserOperationClaimEntity>, GetListResponse<GetListUserOperationClaimResponse>>();

        config.NewConfig<UserOperationClaimEntity, GetListByDynamicUserOperationClaimResponse>()
              .Map(dest => dest.OperationClaimName, src => src.OperationClaim.Name)
              .TwoWays();

        config.NewConfig<Paginate<UserOperationClaimEntity>, GetListResponse<GetListByDynamicUserOperationClaimResponse>>()
              .TwoWays();
    }
}