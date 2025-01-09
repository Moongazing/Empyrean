using FluentValidation;
namespace Moongazing.Empyrean.Application.Features.BankDetails.Commands.Delete;

public class DeleteBankDetailCommandValidator : AbstractValidator<DeleteBankDetailCommand>
{
    public DeleteBankDetailCommandValidator()
    {
        RuleFor(v => v.Id)
            .NotEmpty()
            .NotNull();
    }
}