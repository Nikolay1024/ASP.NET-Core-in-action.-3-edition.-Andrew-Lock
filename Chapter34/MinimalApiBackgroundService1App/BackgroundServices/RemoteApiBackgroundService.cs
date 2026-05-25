using MinimalApiBackgroundService1App.Cache;
using MinimalApiBackgroundService1App.HttpClients;
using MinimalApiBackgroundService1App.Models;

namespace MinimalApiBackgroundService1App.BackgroundServices
{
    // Ќаследуем от BackgroundService, чтобы создать задачу, котора€ будет выполн€тьс€ в течение всего жизненного
    // цикла вашего приложени€.
    public class RemoteApiBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RemoteApiBackgroundService> _logger;
        // ѕростой кеш дл€ сохранени€ данных, запрошенных из удаленного API.
        private readonly RemoteApiCache _cache;

        // ¬недр€ем IServiceProvider, чтобы вы могли создавать экземпл€ры типизированного HTTP клиента.
        public RemoteApiBackgroundService(IServiceProvider serviceProvider,
            ILogger<RemoteApiBackgroundService> logger, RemoteApiCache cache)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _cache = cache;
        }

        // ћетод StartAsync() выполн€етс€ при запуске приложени€, прежде чем начнет работу конвейер обработки
        // HTTP запросов. Ётот метод подходит дл€ предварительного получени€ данных, чтобы конвейер обработки
        // HTTP запросов гарантировано имел данные при первом обращении.
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("»митаци€ 5 секундной задержки при первом обращении к удаленному API.");
            await Task.Delay(TimeSpan.FromSeconds(5), cancellationToken);

            bool success = false;
            // ѕродолжает попытки обновить данные альбомов, пока это не удаетс€.
            while (!success && !cancellationToken.IsCancellationRequested)
                success = await TryUpdateAlbumsAsync();
            // ѕосле успешного обновлени€ запускает фоновый сервис.
            await base.StartAsync(cancellationToken);
        }
        // ћетод ExecuteAsync() запускает основной цикл выполнени€ фонового сервиса.
        // CancellationToken, переданный в качестве аргумента, срабатывает при завершении работы приложени€.
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // ѕродолжает цикл, пока приложение не завершитс€.
            while (!cancellationToken.IsCancellationRequested)
            {
                // ∆дет 15 секунд (или завершени€ приложени€) перед обновлением кеша.
                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
                await TryUpdateAlbumsAsync();
            }
        }

        private async Task<bool> TryUpdateAlbumsAsync()
        {
            try
            {
                _logger.LogInformation("ѕолучение данных из удаленного API.");
                // —оздает новый экземпл€р типизированного HTTP клиента.
                RemoteApiHttpClient httpClient = _serviceProvider.GetRequiredService<RemoteApiHttpClient>();
                // ѕолучает данные из удаленного API.
                List<Album> albums = await httpClient.GetAlbumsAsync();
                // —охран€ет данные в кеш.
                _cache.Albums = albums;
                _logger.LogInformation(" еш обновлен данными из удаленного API.");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ќшибка при обновлении кеша данными из удаленного API.");
                return false;
            }
        }
    }
}
