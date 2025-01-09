using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.BankDetails.Commands.Create;

public class CreateBankDetailCommandValidator : AbstractValidator<CreateBankDetailCommand>
{
    public CreateBankDetailCommandValidator()
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