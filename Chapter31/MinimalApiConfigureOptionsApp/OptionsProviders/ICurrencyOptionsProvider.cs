namespace MinimalApiConfigureOptionsApp.OptionsProviders
{
    public interface ICurrencyOptionsProvider
    {
        string[] GetSupportedCurrencies();
    }
}
