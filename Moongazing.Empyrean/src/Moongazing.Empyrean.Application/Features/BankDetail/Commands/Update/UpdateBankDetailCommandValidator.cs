using FluentValidation;
namespace Moongazing.Empyrean.Application.Features.BankDetail.Commands.Update;

public class UpdateBankDetailCommandValidator : AbstractValidator<UpdateBankDetailCommand>
{
    public UpdateBankDetailCommandValidator()
    {
        RuleFor(p => p.BankName)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(p => p.AccountNumber)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(p => p.IBAN)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .NotNull()
            .MaximumLength(100);

        RuleFor(p => p.EmployeeId)
            .NotEmpty()
            .NotNull();
    }
}