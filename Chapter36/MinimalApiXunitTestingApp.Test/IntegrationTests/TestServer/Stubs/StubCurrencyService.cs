using MinimalApiXunitTestingApp.Services;

namespace MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.Stubs
{
    public class StubCurrencyService : ICurrencyService
    {
        public decimal ConvertToGbp(decimal quantity, decimal exchangeRate, int decimalPlaces)
        {
            return 3;
        }
    }
}
