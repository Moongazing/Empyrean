using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Delete;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Update;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetById;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetList;
using Moongazing.Empyrean.Application.Features.OperationClaims.Queries.GetListByDynamic;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;

namespace Moongazing.Empyrean.WebApi.Controllers;

[Route("api/empyrean/operationclaims")]
[ApiController]
public sealed class OperationClaimsController : BaseController
{

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CreateOperationClaimCommand createOperationClaimCommand)
    {
        CreateOperationClaimResponse result = await Sender.Send(createOperationClaimCommand).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Update([FromBody] UpdateOperationClaimCommand updateOperationClaimCommand)
    {
        UpdateOperationClaimResponse result = await Sender.Send(updateOperationClaimCommand).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteOperationClaimCommand deleteOperationClaimCommand)
    {
        DeleteOperationClaimResponse result = await Sender.Send(deleteOperationClaimCommand).ConfigureAwait(false);
        return Ok(result);
    }
    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdOperationClaimQuery getByIdOperationClaimQuery = new() { Id = id };
        GetByIdOperationClaimResponse result = await Sender.Send(getByIdOperationClaimQuery).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("getlist")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListOperationClaimQuery getListOperationClaimQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListOperationClaimResponse> result = await Sender.Send(getListOperationClaimQuery).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamicQuery)
    {
        GetListByDynamicOperationClaimQuery getListByDynamicOperationClaimQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListByDynamicOperationClaimResponse> result = await Sender.Send(getListByDynamicOperationClaimQuery).ConfigureAwait(false);
        return Ok(result);
    }
}
