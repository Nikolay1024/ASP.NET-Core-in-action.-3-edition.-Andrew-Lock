using WorkerServiceApp.Data;
using WorkerServiceApp.HttpClients;

namespace WorkerServiceApp.BackgroundServices
{
    // Фоновый сервис (Сервис рабочей роли/Worker service) наследуется от BackgroundService.
    public class RemoteApiBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RemoteApiBackgroundService> _logger;

        public RemoteApiBackgroundService(IServiceProvider serviceProvider,
            ILogger<RemoteApiBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        // Метод ExecuteAsync() запускает основной цикл выполнения фонового сервиса.
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Создает новый экземпляр типизированного HTTP клиента.
                RemoteApiHttpClient httpClient = _serviceProvider.GetRequiredService<RemoteApiHttpClient>();

                // Создает область (scope) с помощью корневого (root) IServiceProvider.
                using IServiceScope scope = _serviceProvider.CreateScope();
                // Область имеет IServiceProvider, который дает доступ к сервисам с жизненным циклом Scoped.
                AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                try
                {
                    _logger.LogInformation("Получение данных из удаленного API.");
                    List<Album> albums = await httpClient.GetAlbumsAsync();
                    
                    // Добавляет либо обновляет данные в БД.
                    await dbContext.BulkMergeAsync(albums);
                    await dbContext.SaveChangesAsync(cancellationToken);
                    _logger.LogInformation("БД обновлена данными из удаленного API.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при обновлении БД данными из удаленного API.");
                }

                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            }
        }
    }
}
