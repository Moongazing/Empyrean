using FluentValidation;
using Moongazing.Empyrean.Application.Features.OperationClaims.Commands.Create;

namespace Doing.Retail.Application.Features.OperationClaims.Validators;

public class CreateOperationClaimCommandValidator : AbstractValidator<CreateOperationClaimCommand>
{
    public CreateOperationClaimCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2);
    }
}