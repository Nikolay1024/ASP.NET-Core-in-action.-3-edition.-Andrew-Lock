using System.ComponentModel.DataAnnotations;

namespace RazorPagesModelValidation2App.ValidationAttributes
{
    public class CurrencyCodeAttribute : ValidationAttribute
    {
        readonly string[] _allowedCodes;

        public CurrencyCodeAttribute(params string[] allowedCodes) => _allowedCodes = allowedCodes;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string? code = value as string;
            if (code == null || !_allowedCodes.Contains(code))
                return new ValidationResult("Недействительный код валюты.");

            return ValidationResult.Success;
        }
    }
}
