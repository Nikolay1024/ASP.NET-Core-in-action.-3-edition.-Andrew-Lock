using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WorkerServiceApp.BackgroundServices;
using WorkerServiceApp.Data;
using WorkerServiceApp.HttpClients;
using WorkerServiceApp.Options;

// Создает IHostBuilder с использованием класса Host.
IHost host = Host.CreateDefaultBuilder(args)
    // Конфигурирует ваш контейнер внедрения зависимостей, добавляя сервисы в IServiceCollection.
    .ConfigureServices((HostBuilderContext hostContext, IServiceCollection services) =>
    {
        services.AddOptions<RemoteApiOptions>().BindConfiguration("RemoteApi");

        // Доступ к IConfiguration можно получить через объект HostBuilderContext.
        string connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

        services.AddHttpClient<RemoteApiHttpClient>(
            (IServiceProvider serviceProvider, HttpClient httpClient) =>
            {
                IOptions<RemoteApiOptions> options =
                    serviceProvider.GetRequiredService<IOptions<RemoteApiOptions>>();
                httpClient.BaseAddress = new Uri(options.Value.Url);
            });

        // Регистрация фонового сервиса (Сервиса рабочей роли/Worker service) RemoteApiBackgroundService с
        // жизненным циклом Singleton.
        services.AddHostedService<RemoteApiBackgroundService>();
    })
    // Добавляет поддержку для запуска приложения в качестве демона systemd Linux.
    .UseSystemd()
    // Добавляет поддержку для запуска приложения в качестве службы Windows.
    .UseWindowsService()
    // Собирает экземпляр IHost.
    .Build();

// Запускает фоновый сервис (Сервиса рабочей роли/Worker service) и ожидает завершения работы.
host.Run();
