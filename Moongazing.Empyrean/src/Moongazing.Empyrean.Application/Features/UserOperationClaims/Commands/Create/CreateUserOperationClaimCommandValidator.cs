using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Create;

public class CreateUserOperationClaimCommandValidator : AbstractValidator<CreateUserOperationClaimCommand>
{
    public CreateUserOperationClaimCommandValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();

        RuleFor(x => x.OperationClaimId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}
