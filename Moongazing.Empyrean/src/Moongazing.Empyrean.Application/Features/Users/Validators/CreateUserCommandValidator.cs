using FluentValidation;
using Moongazing.Empyrean.Application.Features.Users.Commands.Create;

namespace Doing.Retail.Application.Features.Users.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email address is required.")
            .EmailAddress().WithMessage("Please provide a valid email address.");

        RuleFor(u => u.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.")
            .MaximumLength(40).WithMessage("First name must not exceed 40 characters.");

        RuleFor(u => u.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.")
            .MaximumLength(40).WithMessage("Last name must not exceed 40 characters.");

        RuleFor(c => c.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(15)
            .Matches(@"(?=.*[A-Z])").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"(?=.*\d)").WithMessage("Password must contain at least one digit.")
            .Matches(@"(?=.*[!@#$%^&*()\-_=+{};:,<.>])").WithMessage("Password must contain at least one special character.");
    }
}
