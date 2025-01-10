using Moongazing.Kernel.Application.Responses;

namespace Moongazing.Empyrean.Application.Features.BankDetail.Commands.Update;

public class UpdateBankDetailResponse : IResponse
{
    public Guid Id { get; set; }
    public string BankName { get; set; } = default!;
    public string AccountNumber { get; set; } = default!;
    public string IBAN { get; set; } = default!;
    public Guid EmployeeId { get; set; }
    public Guid UpdatedBy { get; set; }
    public DateTime UpdatedDate { get; set; }
}
