using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Jwt;

namespace Doing.Retail.Application.Features.Users.Commands.UpdateFromAuth;

public class UpdateUserFromAuthResponse : IResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public AccessToken AccessToken { get; set; } = null!;

}

