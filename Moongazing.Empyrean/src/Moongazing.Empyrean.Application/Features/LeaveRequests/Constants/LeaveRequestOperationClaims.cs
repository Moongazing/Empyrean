using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moongazing.Empyrean.Application.Features.LeaveRequests.Constants;

public static class LeaveRequestOperationClaims
{
    public const string Admin = "leaverequests.admin";
    public const string Read = "leaverequests.read";
    public const string Write = "leaverequests.write";
    public const string Add = "leaverequests.add";
    public const string Update = "leaverequests.update";
    public const string Delete = "leaverequests.delete";
}
