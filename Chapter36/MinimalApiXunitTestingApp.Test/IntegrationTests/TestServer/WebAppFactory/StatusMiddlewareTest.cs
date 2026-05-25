using Microsoft.AspNetCore.Mvc.Testing;

namespace MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.WebAppFactory
{
    // Интеграционное тестирование с использованием WebApplicationFactory, который внутри использует TestServer.
    // Интерфейс-маркер - это интерфейс, не содержищий членов, которые необходимо реализовывать.
    // Тестовый класс должен реализовывать интерфейс-маркер IClassFixture<F>, где F - это WebApplicationFactory<P>.
    // А P - это класс, из которого с помощью рефлексии извлекаются данные конфигурации тестируемого приложения
    // (конфигурация, логирование, конвейер промежутчного ПО, контейнер внедрения зависимостей).
    public class StatusMiddlewareTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _fixture;

        public StatusMiddlewareTest(WebApplicationFactory<Program> fixture) => _fixture = fixture;

        [Fact]
        public async Task ForMatchingRequest()
        {
            // Создает HttpClient, который отправляет запросы к TestServer в памяти.
            HttpClient httpClient = _fixture.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync("/ping");

            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Equal("pong", content);
        }
    }
}
