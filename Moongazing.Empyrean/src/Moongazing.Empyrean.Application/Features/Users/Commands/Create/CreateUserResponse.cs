using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Models.Enums;

namespace Doing.Retail.Application.Features.Users.Commands.Create;

public class CreateUserResponse : IResponse
{
    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public UserStatus Status { get; set; }

}
