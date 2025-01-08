using Doing.Retail.Application.Features.UserOperationClaims.Commands.Create;
using FluentValidation;

namespace Doing.Retail.Application.Features.UserOperationClaims.Validators;

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
