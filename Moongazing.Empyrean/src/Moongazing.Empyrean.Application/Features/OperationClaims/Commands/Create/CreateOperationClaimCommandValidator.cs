using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;

public class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommand>
{
    public CreateOperationClaimCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
    }
}