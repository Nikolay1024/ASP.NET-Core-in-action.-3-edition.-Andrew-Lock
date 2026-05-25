using MinimalApiXunitTestingApp.BindingModels;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.CustomWebAppFactory
{
    // Реализует интерфейс IClassFixture для пользовательской фабрики веб-приложений CustomWebAppFactory.
    public class CurrencyEndpointTest : IClassFixture<CustomWebAppFactory>
    {
        private readonly CustomWebAppFactory _fixture;

        public CurrencyEndpointTest(CustomWebAppFactory fixture) => _fixture = fixture;

        [Fact]
        public async Task PostCurrency_UsualCase()
        {
            // Создает HttpClient, который отправляет запросы к TestServer в памяти.
            HttpClient httpClient = _fixture.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(new CurrencyBindingModel()
            {
                Quantity = 10,
                ExchangeRate = 3,
                DecimalPlaces = 2,
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

            HttpResponseMessage response = await httpClient.PostAsync("currency", requestContent);

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("3", responseContent);
        }
    }
}
