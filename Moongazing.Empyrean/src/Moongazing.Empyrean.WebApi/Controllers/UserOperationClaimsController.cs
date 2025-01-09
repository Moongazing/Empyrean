using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Create;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Delete;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Update;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetById;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetList;
using Moongazing.Empyrean.Application.Features.UserOperationClaims.Queries.GetListDynamic;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;

namespace Doing.Retail.Api.Controllers;

[Route("api/empyrean/useroperationclaims")]
[ApiController]
public sealed class UserOperationClaimsController : BaseController
{

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CreateUserOperationClaimCommand createUserOperationClaimCommand)
    {
        CreateUserOperationClaimResponse result = await Sender.Send(createUserOperationClaimCommand).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Update([FromBody] UpdateUserOperationClaimCommand updateUserOperationClaimCommand)
    {
        UpdateUserOperationClaimResponse result = await Sender.Send(updateUserOperationClaimCommand).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteUserOperationClaimCommand deleteUserOperationClaimCommand)
    {
        DeleteUserOperationClaimResponse result = await Sender.Send(deleteUserOperationClaimCommand).ConfigureAwait(false);
        return Ok(result);
    }
    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdUserOperationClaimQuery getByIdUserOperationClaimQuery = new() { Id = id };
        GetByIdUserOperationClaimResponse result = await Sender.Send(getByIdUserOperationClaimQuery).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpGet("getlist")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserOperationClaimQuery getListUserOperationClaimQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListUserOperationClaimResponse> result = await Sender.Send(getListUserOperationClaimQuery).ConfigureAwait(false);
        return Ok(result);
    }

    [HttpPost("search")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery? dynamicQuery = null)
    {
        GetListByDynamicUserOperationClaimQuery getListByDynamicUserOperationClaimQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery! };
        GetListResponse<GetListByDynamicUserOperationClaimResponse> result = await Sender.Send(getListByDynamicUserOperationClaimQuery).ConfigureAwait(false); ;
        return Ok(result);
    }
}
