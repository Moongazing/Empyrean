using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Delete;

public class DeleteUserOperationClaimCommandValidator : AbstractValidator<DeleteUserOperationClaimCommand>
{
    public DeleteUserOperationClaimCommandValidator()
    {
        RuleFor(x => x.Id)
          .Cascade(CascadeMode.Stop)
          .NotEmpty();
    }
}
