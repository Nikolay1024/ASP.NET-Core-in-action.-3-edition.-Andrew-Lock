using Quartz;
using WorkerServiceQuartzApp.Data;
using WorkerServiceQuartzApp.HttpClients;

namespace WorkerServiceQuartzApp.Jobs
{
    // Quartz.NET продолжает запускать новые экземпляры задания при срабатывании триггера, даже если
    // задание уже запущено. Чтобы этого избежать, нужно декорировать задание атрибутом
    // [DisallowConcurrentExecution].
    [DisallowConcurrentExecution]
    // Задания Quartz.NET должны реализовывать интерфейс IJob.
    // Новый экземпляр IJob создается каждый раз при выполнении задания.
    public class RemoteApiJob : IJob
    {
        private readonly ILogger<RemoteApiJob> _logger;
        private readonly AppDbContext _dbContext;
        private readonly RemoteApiHttpClient _httpClient;

        // Т.к. IJob имеет жизненный цикл Scoped, доступно внедрение зависимостей с жизненным циклом Singleton и
        // Scoped.
        public RemoteApiJob(ILogger<RemoteApiJob> logger, AppDbContext dbContext, RemoteApiHttpClient httpClient)
        {
            _logger = logger;
            _dbContext = dbContext;
            _httpClient = httpClient;
        }

        // IJob требует, чтобы вы реализовали метод Execute().
        // Метод Execute() выполняется каждый раз при выполнении задания.
        public async Task Execute(IJobExecutionContext jobContext)
        {
            try
            {
                _logger.LogInformation("Получение данных из удаленного API.");
                List<Album> albums = await _httpClient.GetAlbumsAsync();

                // Добавляет либо обновляет данные в БД.
                await _dbContext.BulkMergeAsync(albums);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("БД обновлена данными из удаленного API.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обновлении БД данными из удаленного API.");
            }
        }
    }
}
