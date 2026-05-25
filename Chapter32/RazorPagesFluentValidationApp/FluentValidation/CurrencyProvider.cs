namespace RazorPagesFluentValidationApp.FluentValidation
{
    public class CurrencyProvider : ICurrencyProvider
    {
        public string[] GetCurrencies()
        {
            return new[] { "GBP", "USD", "CAD", "EUR" };
        }
    }
}
