namespace RazorPagesXunitTestingApp.Services
{
    public interface ICurrencyService
    {
        decimal ConvertToGbp(decimal quantity, decimal exchangeRate, int decimalPlaces);
    }
}
