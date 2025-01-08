using FluentValidation;
using Moongazing.Empyrean.Application.Features.Users.Commands.Delete;

namespace Doing.Retail.Application.Features.Users.Validators;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {

    }
}