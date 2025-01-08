using Doing.Retail.Application.Features.OperationClaims.Commands.Update;
using FluentValidation;

namespace Doing.Retail.Application.Features.OperationClaims.Validators;

public class UpdateOperationClaimCommandValidator : AbstractValidator<UpdateOperationClaimCommand>
{
    public UpdateOperationClaimCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
    }
}
