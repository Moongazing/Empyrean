using Moongazing.Kernel.Application.Responses;

namespace Doing.Retail.Application.Features.Users.Commands.Update;

public class UpdateUserResponse : IResponse
{

    public int Id { get; set; }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public bool Status { get; set; }

}
