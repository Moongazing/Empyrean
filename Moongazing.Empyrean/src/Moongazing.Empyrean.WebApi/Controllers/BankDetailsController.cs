﻿using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.BankDetail.Commands.Update;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetByEmployeeId;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetById;
using Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetList;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Delete;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.WebApi.Controllers;

[Route("api/empyrean/bankdetails")]
[ApiController]
public sealed class BankDetailsController : BaseController
{

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CreateBankDetailCommand createBankDetailCommand)
    {
        CreateBankDetailResponse result = await Sender.Send(createBankDetailCommand).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteBankDetailCommand deleteBankDetailCommand)
    {
        DeleteBankDetailResponse result = await Sender.Send(deleteBankDetailCommand).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateBankDetailCommand updateBankDetailCommand)
    {
        UpdateBankDetailResponse result = await Sender.Send(updateBankDetailCommand).ConfigureAwait(false);
        return Ok(result);
    }
    [HttpGet("details/{employeeId:uuid}")]
    public async Task<IActionResult> GetDetailByEmployeeId(Guid employeeId)
    {
        GetBankDetailByEmployeeIdQuery getBankDetailByEmployeeIdQuery = new() { EmployeeId = employeeId };
        GetBankDetailByEmployeeIdResponse result = await Sender.Send(getBankDetailByEmployeeIdQuery).ConfigureAwait(false);
        return Ok(result);

    }
    [HttpGet("details/{bankDetailId:uuid}")]
    public async Task<IActionResult> GetDetailById(Guid bankDetailId)
    {
        GetBankDetailByIdQuery getBankDetailByIdQuery = new() { Id = bankDetailId };
        GetBankDetailByIdResponse result = await Sender.Send(getBankDetailByIdQuery).ConfigureAwait(false);
        return Ok(result);
    }
    [HttpGet("getlist")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetBankDetailListQuery getBankDetailListQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetBankDetailListResponse> result = await Sender.Send(getBankDetailListQuery).ConfigureAwait(false);
        return Ok(result);
    }
}
