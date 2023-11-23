using System.ComponentModel.DataAnnotations;

namespace Ingestion.Application.Validators;

public class DateTimeValidationAttribute : ValidationAttribute
{
    private readonly string _expectedFormat;

    public DateTimeValidationAttribute(string expectedFormat)
    {
        _expectedFormat = expectedFormat;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return ValidationResult.Success!;
        }

        if (value is not string dateString || !DateTime.TryParseExact(dateString, _expectedFormat, null, System.Globalization.DateTimeStyles.None, out _))
        {
            return new ValidationResult(ErrorMessage ?? $"Invalid date format. Expected format: {_expectedFormat}");
        }

        return ValidationResult.Success!;
    }
}
