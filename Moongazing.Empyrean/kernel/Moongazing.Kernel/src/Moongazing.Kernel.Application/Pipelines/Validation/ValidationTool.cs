﻿using FluentValidation;
using FluentValidation.Results;

namespace Moongazing.Kernel.Application.Pipelines.Validation;

public static class ValidationTool
{
    public static void Validate(IValidator validator, object entity)
    {
        ValidationContext<object> context = new(entity);
        ValidationResult result = validator.Validate(context);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}