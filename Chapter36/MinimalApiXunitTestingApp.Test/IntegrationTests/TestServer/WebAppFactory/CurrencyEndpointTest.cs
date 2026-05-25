using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApiXunitTestingApp.BindingModels;
using MinimalApiXunitTestingApp.Services;
using MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.Stubs;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.WebAppFactory
{
    // Интеграционное тестирование с использованием WebApplicationFactory, который внутри использует TestServer.
    public class CurrencyEndpointTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;

        public CurrencyEndpointTest(WebApplicationFactory<Program> fixture) => _fixture = fixture;

        [Fact]
        // Замена зависимостей в классе WebApplicationFactory.
        public async Task CurrencyEndpoint_UsualCase()
        {
            // Метод WithWebHostBuilder() создает свою фабрику веб-приложения с дополнительной конфигурацией.
            WebApplicationFactory<Program> webAppFactory = _fixture.WithWebHostBuilder(hostBuilder =>
            {
                // ConfigureTestServices() выполняется после того, как все сервисы внедрения зависимостей будут
                // настроены как в вашем тестируемом приложении.
                hostBuilder.ConfigureTestServices(services =>
                {
                    // Удаляет все реализации ICurrencyService из контейнера внедрения зависимостей.
                    services.RemoveAll<ICurrencyService>();
                    // Для сервиса ICurrencyService добавляет реализацию-заглушку.
                    services.AddSingleton<ICurrencyService, StubCurrencyService>();
                });
            });

            // Создает HttpClient, который отправляет запросы к TestServer в памяти.
            HttpClient httpClient = webAppFactory.CreateClient();
            var requestContent = new StringContent(JsonSerializer.Serialize(new CurrencyBindingModel()
            {
                Quantity = 10,
                ExchangeRate = 3,
                DecimalPlaces = 2,
            }),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);

            HttpResponseMessage response = await httpClient.PostAsync("/currency", requestContent);

            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();

            Assert.Equal("3", responseContent);
        }
    }
}
