using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Create;
using Moongazing.Empyrean.WebApi.Controllers.Common;

[Route("api/empyrean/leaverequests")]
[ApiController]
public sealed class LeaveRequestsController : BaseController
{

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CreateLeaveRequestCommand createLeaveRequestCommand)
    {
        CreateLeaveRequestResponse result = await Sender.Send(createLeaveRequestCommand).ConfigureAwait(false);
        return Ok(result);
    }
}