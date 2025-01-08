using Doing.Retail.Application.Features.Users.Commands.Create;
using Doing.Retail.Application.Features.Users.Commands.Update;
using Doing.Retail.Application.Features.Users.Commands.UpdateFromAuth;
using Doing.Retail.Application.Features.Users.Queries.GetById;
using Doing.Retail.Application.Features.Users.Queries.GetList;
using Doing.Retail.Application.Features.Users.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.Users.Commands.Create;
using Moongazing.Empyrean.Application.Features.Users.Commands.Delete;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;

namespace Moongazing.Empyrean.WebApi.Controllers;

[Route("api/empyrean/users")]
[ApiController]
public sealed class UsersController : BaseController
{

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] CreateUserCommand createUserCommand)
    {
        CreateUserResponse result = await Sender.Send(createUserCommand).ConfigureAwait(false);
        return Created(string.Empty, result);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand updateUserCommand)
    {
        UpdateUserResponse result = await Sender.Send(updateUserCommand).ConfigureAwait(false); ;
        return Ok(result);
    }

    [HttpPut("currentuser/edit")]
    public async Task<IActionResult> UpdateFromAuth([FromBody] UpdateUserFromAuthCommand updateUserFromAuthCommand)
    {
        updateUserFromAuthCommand.Id = GetUserIdFromRequest();
        UpdateUserFromAuthResponse result = await Sender.Send(updateUserFromAuthCommand).ConfigureAwait(false); ;
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand deleteUserCommand)
    {
        DeleteUserResponse result = await Sender.Send(deleteUserCommand).ConfigureAwait(false); ;
        return Ok(result);
    }
    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        GetByIdUserQuery getByIdUserQuery = new() { Id = id };
        GetByIdUserResponse result = await Sender.Send(getByIdUserQuery).ConfigureAwait(false); ;
        return Ok(result);
    }

    [HttpGet("currentuser/profile")]
    public async Task<IActionResult> GetFromAuth()
    {
        GetByIdUserQuery getByIdUserQuery = new() { Id = GetUserIdFromRequest() };
        GetByIdUserResponse result = await Sender.Send(getByIdUserQuery).ConfigureAwait(false); ;
        return Ok(result);
    }

    [HttpGet("getlist")]
    public async Task<IActionResult> GetList([FromQuery] PageRequest pageRequest)
    {
        GetListUserQuery getListUserQuery = new() { PageRequest = pageRequest };
        GetListResponse<GetListUserResponse> result = await Sender.Send(getListUserQuery).ConfigureAwait(false); ;
        return Ok(result);
    }
    [HttpPost("search")]
    public async Task<IActionResult> GetListByDynamic([FromQuery] PageRequest pageRequest, [FromBody] DynamicQuery dynamicQuery)
    {
        GetListByDynamicUserQuery getListByDynamicUserQuery = new() { PageRequest = pageRequest, DynamicQuery = dynamicQuery };
        GetListResponse<GetListByDynamicUserResponse> result = await Sender.Send(getListByDynamicUserQuery).ConfigureAwait(false); ;
        return Ok(result);
    }
}
