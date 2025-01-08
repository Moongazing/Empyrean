using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Security.Models.Enums;

namespace Doing.Retail.Application.Features.Users.Queries.GetList;

public class GetListUserResponse : IResponse
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public UserStatus Status { get; set; }

}
