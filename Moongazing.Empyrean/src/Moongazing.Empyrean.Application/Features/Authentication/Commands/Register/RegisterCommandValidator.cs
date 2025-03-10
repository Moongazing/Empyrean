﻿using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.Authentication.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserRegisterDto.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(x => x.UserRegisterDto.LastName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(x => x.UserRegisterDto.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.UserRegisterDto.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(7)
            .MaximumLength(20)
            .Matches(Domain.Constants.Constants.PasswordRegex);
    }
}

