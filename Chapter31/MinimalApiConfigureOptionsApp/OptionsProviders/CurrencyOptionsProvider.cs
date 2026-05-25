namespace MinimalApiConfigureOptionsApp.OptionsProviders
{
    public class CurrencyOptionsProvider : ICurrencyOptionsProvider
    {
        public string[] GetSupportedCurrencies()
        {
            // Здесь может быть загрузка настроек из базы данных/удаленного API.
            return new string[] { "GBP", "USD", "EUR", "CAD" };
        }
    }
}
