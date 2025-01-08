using Doing.Retail.Application.Features.UserOperationClaims.Commands.Update;
using FluentValidation;

namespace Doing.Retail.Application.Features.UserOperationClaims.Validators;

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
