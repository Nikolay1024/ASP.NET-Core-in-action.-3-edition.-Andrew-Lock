using FluentValidation;

namespace RazorPagesFluentValidationApp.FluentValidation
{
    public static class ValidationExtensions
    {
        public static IRuleBuilderOptions<T, string> MustBeCurrencyCode<T>(
            this IRuleBuilder<T, string> ruleBuilder, string[] allowedCurrencies)
        {
            return ruleBuilder
                .Must(currency => allowedCurrencies.Contains(currency))
                .WithMessage("Недействительный код валюты");
        }

        public static IRuleBuilderOptions<T, string> MustBeCurrencyCode<T>(
            this IRuleBuilder<T, string> ruleBuilder, ICurrencyProvider currencyProvider)
        {
            return ruleBuilder
                .Must(currencyCode => currencyProvider.GetCurrencies().Contains(currencyCode))
                .WithMessage("Недействительный код валюты");
        }
    }
}
