using Doing.Retail.Application.Features.Users.Commands.Create;
using Doing.Retail.Application.Features.Users.Commands.Update;
using Doing.Retail.Application.Features.Users.Commands.UpdateFromAuth;
using Doing.Retail.Application.Features.Users.Queries.GetById;
using Doing.Retail.Application.Features.Users.Queries.GetList;
using Doing.Retail.Application.Features.Users.Queries.GetListByDynamic;
using Microsoft.AspNetCore.Mvc;
using Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;
using Moongazing.Empyrean.Application.Features.Users.Commands.Create;
using Moongazing.Empyrean.Application.Features.Users.Commands.Delete;
using Moongazing.Empyrean.WebApi.Controllers.Common;
using Moongazing.Kernel.Application.Requests;
using Moongazing.Kernel.Application.Responses;
using Moongazing.Kernel.Persistence.Dynamic;

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

   
}
