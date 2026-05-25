using MinimalApiBackgroundService2App.Data;
using MinimalApiBackgroundService2App.HttpClients;

namespace MinimalApiBackgroundService2App.BackgroundServices
{
    // Наследуем от BackgroundService, чтобы создать задачу, которая будет выполняться в течение всего жизненного
    // цикла вашего приложения.
    public class RemoteApiBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<RemoteApiBackgroundService> _logger;

        // Внедряем IServiceProvider, чтобы вы могли создавать экземпляры типизированного HTTP клиента.
        // Внедренный IServiceProvider можно использовать для получения сервисов с жизненным циклом Singleton
        // или для создания областей (scopes).
        public RemoteApiBackgroundService(IServiceProvider serviceProvider,
            ILogger<RemoteApiBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        // Метод StartAsync() выполняется при запуске приложения, прежде чем начнет работу конвейер обработки
        // HTTP запросов. Этот метод подходит для предварительного получения данных, чтобы конвейер обработки
        // HTTP запросов гарантировано имел данные при первом обращении.
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            bool success = false;
            // Продолжает попытки обновить данные альбомов, пока это не удается.
            while (!success && !cancellationToken.IsCancellationRequested)
            {
                success = await TryUpdateAlbumsAsync(cancellationToken);
                _logger.LogInformation("Имитация 15 секундной задержки при первом обращении к удаленному API.");
                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            }
            // После успешного обновления запускает фоновый сервис.
            await base.StartAsync(cancellationToken);
        }
        // Метод ExecuteAsync() запускает основной цикл выполнения фонового сервиса.
        // CancellationToken, переданный в качестве аргумента, срабатывает при завершении работы приложения.
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            // Продолжает цикл, пока приложение не завершится.
            while (!cancellationToken.IsCancellationRequested)
            {
                await TryUpdateAlbumsAsync(cancellationToken);
                // Ждет 15 секунд (или завершения приложения) перед обновлением кеша.
                await Task.Delay(TimeSpan.FromSeconds(15), cancellationToken);
            }
        }

        private async Task<bool> TryUpdateAlbumsAsync(CancellationToken cancellationToken)
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
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении БД данными из удаленного API.");
                return false;
            }
        }
    }
}
