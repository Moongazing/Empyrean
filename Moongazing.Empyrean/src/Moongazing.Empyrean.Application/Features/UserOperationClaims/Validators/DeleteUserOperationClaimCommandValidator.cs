using Doing.Retail.Application.Features.UserOperationClaims.Commands.Delete;
using FluentValidation;

namespace Doing.Retail.Application.Features.UserOperationClaims.Validators;

public class DeleteUserOperationClaimCommandValidator : AbstractValidator<DeleteUserOperationClaimCommand>
{
    public DeleteUserOperationClaimCommandValidator()
    {
        RuleFor(x => x.Id)
          .Cascade(CascadeMode.Stop)
          .NotEmpty();
    }
}
