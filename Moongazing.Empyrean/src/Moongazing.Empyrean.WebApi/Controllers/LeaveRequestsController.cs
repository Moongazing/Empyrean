﻿using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetList;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Create;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Delete;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Commands.Update;
using Moongazing.Empyrean.Application.Features.LeaveRequests.Queries.GetTodayList;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;

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
    public async Task<IActionResult> Delete([FromBody] DeleteLeaveRequestCommand deleteLeaveRequestCommand)
    {
        DeleteLeaveRequestResponse result = await Sender.Send(deleteLeaveRequestCommand).ConfigureAwait(false);
        return Ok(result);
    }
    [HttpPut("edit")]
    public async Task<IActionResult> Edit([FromBody] UpdateLeaveRequestCommand editLeaveRequestCommand)
    {
        UpdateLeaveRequestResponse result = await Sender.Send(editLeaveRequestCommand).ConfigureAwait(false);
        return Ok(result);
    }
    [HttpGet("getlist/today")]
    public async Task<IActionResult> GetListToday([FromQuery] PageRequest pageRequest)
    {
        GetLeaveRequestListByTodayQuery getTodayLeaveRequestListQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetLeaveRequestListByTodayResponse> result = await Sender.Send(getTodayLeaveRequestListQuery).ConfigureAwait(false);
        return Ok(result);
    }
}