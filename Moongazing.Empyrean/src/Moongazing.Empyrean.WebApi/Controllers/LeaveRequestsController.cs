﻿using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Create;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Delete;
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

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete ([FromBody] DeleteLeaveRequestCommand deleteLeaveRequestCommand)
    {
        DeleteLeaveRequestResponse result = await Sender.Send(deleteLeaveRequestCommand).ConfigureAwait(false);
        return Ok(result);
    }
}