using FluentValidation;

namespace Moongazing.Empyrean.Application.Features.UserOperationClaims.Commands.Update;

public class UpdateUserOperationClaimCommandValidator : AbstractValidator<UpdateUserOperationClaimCommand>
{
    public UpdateUserOperationClaimCommandValidator()
    {
        RuleFor(x => x.Id)
           .Cascade(CascadeMode.Stop)
           .NotEmpty();

        RuleFor(x => x.UserId)
           .Cascade(CascadeMode.Stop)
           .NotEmpty();

        RuleFor(x => x.OperationClaimId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}
