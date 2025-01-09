using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;

public class CreateBankDetailResponse : IResponse
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = default!;
    public string AccountNumber { get; set; } = default!;
    public string IBAN { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }

}
