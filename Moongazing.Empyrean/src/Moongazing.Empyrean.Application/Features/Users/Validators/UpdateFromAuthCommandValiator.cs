using Doing.Retail.Application.Features.Users.Commands.UpdateFromAuth;
using FluentValidation;

namespace Doing.Retail.Application.Features.Users.Validators;

public class UpdateFromAuthCommandValidator : AbstractValidator<UpdateUserFromAuthCommand>
{
    public UpdateFromAuthCommandValidator()
    {
        RuleFor(c => c.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("First name is required.")
            .MinimumLength(2).WithMessage("First name must be at least 2 characters long.");

        RuleFor(c => c.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Last name is required.")
            .MinimumLength(2).WithMessage("Last name must be at least 2 characters long.");

        RuleFor(c => c.NewPassword)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("New password is required.")
            .MinimumLength(15).WithMessage("New password must be at least 15 characters long.")
            .Matches(@"(?=.*[A-Z])").WithMessage("New password must contain at least one uppercase letter.")
            .Matches(@"(?=.*\d)").WithMessage("New password must contain at least one digit.")
            .Matches(@"(?=.*[!@#$%^&*()\-_=+{};:,<.>])").WithMessage("New password must contain at least one special character.");

    }
}
