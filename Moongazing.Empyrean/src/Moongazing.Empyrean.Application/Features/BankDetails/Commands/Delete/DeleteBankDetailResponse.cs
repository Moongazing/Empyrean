using Moongazing.Kernel.Application.Responses;

public class DeleteBankDetailResponse : IResponse
{
    public Guid Id { get; set; }
    public Guid DeletedBy { get; set; }
    public DateTime DeletedDate { get; set; }
}