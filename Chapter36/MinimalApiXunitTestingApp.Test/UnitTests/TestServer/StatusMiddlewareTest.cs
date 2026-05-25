using Microsoft.AspNetCore.TestHost;
using MinimalApiXunitTestingApp.Middlewares;

namespace MinimalApiXunitTestingApp.Test.UnitTests.TestServer
{
    // Модульное тестирование промежуточного ПО StatusMiddleware с использование TestServer.
    public class StatusMiddlewareTest
    {
        [Fact]
        // Подход с использованием настройки обобщенного хоста (generic host).
        public async Task ForMatchingRequest1()
        {
            // Настраивает HostBuilder для определения тестового приложения в памяти.
            IHostBuilder hostBuilder = new HostBuilder().ConfigureWebHost(webHost =>
            {
                // Настраивает хост на использование TestServer в памяти вместо веб-сервера Kestrel.
                webHost.UseTestServer();
                // Добавляет StatusMiddleware в качестве единственного промежуточного ПО в конвейере HTTP.
                webHost.Configure(app => app.UseMiddleware<StatusMiddleware>());
            });

            // Собирает и запускаем хост.
            IHost host = await hostBuilder.StartAsync();

            // Создает HttpClient для обращения к тестируемому приложению.
            HttpClient httpClient = host.GetTestClient();

            // Делает запрос в памяти, который обрабатывается приложением, как если бы запрос отправлялся по HTTP.
            HttpResponseMessage response = await httpClient.GetAsync("/ping");

            // Проверяет, что ответ был успешным: код состояния – 2xx.
            response.EnsureSuccessStatusCode();
            // Считывает тело ответа.
            string content = await response.Content.ReadAsStringAsync();

            // Проверяет тело ответа.
            Assert.Equal("pong", content);
        }

        [Fact]
        // Подход с использованием настройки минимального хостинга (minimal hosting).
        public async Task ForMatchingRequest2()
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();
            WebApplication app = builder.Build();
            app.UseMiddleware<StatusMiddleware>();
            await app.StartAsync();

            HttpClient httpClient = app.GetTestClient();
            HttpResponseMessage response = await httpClient.GetAsync("/ping");

            response.EnsureSuccessStatusCode();
            string content = await response.Content.ReadAsStringAsync();

            Assert.Equal("pong", content);
        }
    }
}
