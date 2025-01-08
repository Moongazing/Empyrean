using AutoMapper;
using Moongazing.Empyrean.Application.Features.Authentication.Commands.Revoke;
using Moongazing.Kernel.Security.Models;

namespace Moongazing.Empyrean.Application.Features.Authentication.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RefreshTokenEntity, RevokeTokenResponse>().ReverseMap();
    }
}
