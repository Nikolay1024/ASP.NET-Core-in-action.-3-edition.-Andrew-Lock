using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MinimalApiXunitTestingApp.Data;
using MinimalApiXunitTestingApp.Services;
using MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.Stubs;

namespace MinimalApiXunitTestingApp.Test.IntegrationTests.TestServer.CustomWebAppFactory
{
    // Создание класса CustomWebAppFactory для уменьшения дублирования кода.
    public class CustomWebAppFactory : WebApplicationFactory<Program>
    {
        private SqliteConnection _connection = null!;

        // Есть много методов, которые можно переопределить. Этот эквивалентен вызову WithWebHostBuilder().
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            builder.ConfigureTestServices(services =>
            {
                // Удаляет все реализации ICurrencyService из контейнера внедрения зависимостей.
                services.RemoveAll<ICurrencyService>();
                // Для сервиса ICurrencyService добавляет реализацию-заглушку.
                services.AddSingleton<ICurrencyService, StubCurrencyService>();

                // Удаляет сервисы для работы с реальной БД из контейнера внедрения зависимостей.
                services.RemoveAll<DbContextOptions<AppDbContext>>();
                // Добавляет сервис для работы с БД в памяти для тестирования.
                services.AddDbContext<AppDbContext>(options => options.UseSqlite(_connection));
            });
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            IHost host = base.CreateHost(builder);

            using (IServiceScope scope = host.Services.CreateScope())
            {
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();
            }

            return host;
        }
    }
}
