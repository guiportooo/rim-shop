using FluentValidation.Results;

namespace Shop.Api.HttpIn.Validations;

public static class Extensions
{
    public static IDictionary<string, string[]> ToDictionary(this ValidationResult validationResult) =>
        validationResult
            .Errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.ErrorMessage).ToArray()
            );
}