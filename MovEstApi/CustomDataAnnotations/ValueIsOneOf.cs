using System.ComponentModel.DataAnnotations;

namespace MovEstApi.CustomDataAnnotations;

public class ValueIsOneOf : ValidationAttribute
{
    private readonly IEnumerable<string> _validString;
    private readonly string? _errorMessage;

    public ValueIsOneOf(IEnumerable<string> validString, 
        string? errorMessage = "")
    {
        _validString = validString;
        _errorMessage = errorMessage;
    }

    protected override ValidationResult IsValid(object? value, 
        ValidationContext validationContext)
    {
        if(!_validString.Contains(value?.ToString())){
            return new ValidationResult(_errorMessage);
        }

        return ValidationResult.Success!;
    }
}