using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.BankDetail.Queries.GetByEmployeeId;

public class GetBankDetailByEmployeeIdResponse : IResponse
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = default!;
    public string AccountNumber { get; set; } = default!;
    public string IBAN { get; set; } = default!;
    public Guid EmployeeId { get; set; }
}