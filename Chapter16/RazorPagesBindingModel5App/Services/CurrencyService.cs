namespace RazorPagesBindingModel5App.Services
{
    public class CurrencyService
    {
        public readonly Dictionary<string, decimal> Currencies = new()
        {
            { "GBP", 1.00m },
            { "USD", 1.22m },
            { "CAD", 1.64m },
            { "EUR", 1.15m },
        };
    }
}
