using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Login;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.UserLoginDto.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.UserLoginDto.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(6);
    }
}