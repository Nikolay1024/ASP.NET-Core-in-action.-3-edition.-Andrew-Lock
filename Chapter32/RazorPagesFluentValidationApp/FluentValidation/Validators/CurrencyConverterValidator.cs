using FluentValidation;
using RazorPagesFluentValidationApp.BindingModels;

namespace RazorPagesFluentValidationApp.FluentValidation.Validators
{
    public class CurrencyConverterValidator : AbstractValidator<CurrencyConverterBindingModel>
    {
        private readonly string[] _allowedCurrencies = { "GBP", "USD", "CAD", "EUR" };

        public CurrencyConverterValidator(ICurrencyProvider provider)
        {
            RuleFor(m => m.CurrencyFrom)
                .NotEmpty()
                .Length(3)
                .Must(сurrencyFrom => _allowedCurrencies.Contains(сurrencyFrom))
                .WithMessage("Недействительный код валюты");

            RuleFor(m => m.CurrencyTo)
                .NotEmpty()
                .Length(3)
                .MustBeCurrencyCode(provider)
                .Must((bindingModel, currencyTo) => currencyTo != bindingModel.CurrencyFrom)
                .WithMessage("Невозможно конвертировать валюту в саму себя");

            RuleFor(m => m.Quantity)
                .NotNull()
                .InclusiveBetween(1.00m, 1000.00m);
        }
    }
}
