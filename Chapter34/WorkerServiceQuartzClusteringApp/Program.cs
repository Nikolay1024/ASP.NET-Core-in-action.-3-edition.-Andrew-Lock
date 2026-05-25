using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;
using WorkerServiceQuartzClusteringApp.Data;
using WorkerServiceQuartzClusteringApp.HttpClients;
using WorkerServiceQuartzClusteringApp.Jobs;
using WorkerServiceQuartzClusteringApp.Options;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((HostBuilderContext hostContext, IServiceCollection services) =>
    {
        services.AddOptions<RemoteApiOptions>().BindConfiguration("RemoteApi");

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

        // Регистрация сервиса планироващика заданий Quartz.NET в контейнере внедрения зависимостей.
        services.AddQuartz(options =>
        {
            // Установка уникального имени планировщика.
            options.SchedulerName = "Получение данных из удаленного API.";
            // Для кластеризации необходимо чтобы каждый экземпляр приложения имел уникальный SchedulerId.
            // Значение AUTO устанавливает уникальный SchedulerId.
            options.SchedulerId = "AUTO";

            // Указание использовать встроенный контейнер внедрения зависимостей для создания заданий IJob.
            // Задания создаются с жизненным циклом Scoped.
            options.UseMicrosoftDependencyInjectionJobFactory();

            // Включение долговременного хранения в БД для данных планировщика Quartz.NET.
            // В этой конфигурации Quartz.NET хранит список заданий и триггеров в БД и использует блокировку БД,
            // чтобы гарантировать, что только один экземпляр вашего приложения обрабатывает триггер и запускает
            // соответствующее задание.
            // SQLite не поддерживает примитивы блокировки БД, необходимые для кластеризации. Вы можете
            // использовать SQLite в качестве долговременного хранилища, но не сможете использовать
            // кластеризацию.
            // Quartz.NET хранит данные в вашей БД, но не пытается создавать таблицы, которые использует сам.
            // Нужно вручную добавлять необходимые таблицы.
            options.UsePersistentStore(storeOptions =>
            {
                // Указание использовать SQL Server для долговременного хранения.
                storeOptions.UseSqlServer(connectionString);
                // Включение кластеризации между несколькими экземплярами приложения.
                storeOptions.UseClustering();
                // Рекомендуемая конфигурацию для долговременного хранения.
                storeOptions.UseProperties = true;
                storeOptions.UseJsonSerializer();
            });

            // Создание уникального ключа задания для связи задания с триггером.
            var jobKey = new JobKey("Обновление в БД таблицы альбомов данными из удаленного API.");
            // Добавление задания в контейнер внедрения зависимостей и связывание задания с ключом задания.
            options.AddJob<RemoteApiJob>(jobOptions => jobOptions.WithIdentity(jobKey));

            // Регистрация триггера и связывание триггера с ключом задания.
            options.AddTrigger(triggerOptions => triggerOptions.ForJob(jobKey)
                // Установка уникального имени триггера, которое будет использоваться при журналировании.
                .WithIdentity(jobKey.Name + " Триггер.")
                // Установка запуска триггера при запуске приложения.
                .StartNow()
                // Установка запуска триггера каждую пятницу в 17:30.
                //.WithSchedule(CronScheduleBuilder.WeeklyOnDayAndHourAndMinute(DayOfWeek.Friday, 17, 30))
                // Установка запуска триггера каждые 15 секунд.
                .WithSimpleSchedule(scheduleOptions => scheduleOptions
                    .WithInterval(TimeSpan.FromSeconds(15)).RepeatForever()));
        });

        // Добавление фонового сервиса от Quartz.NET, который запускает планировщик Quartz.NET.
        // Установка ожидания завершения исполняемого задания при завершении работы приложения.
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    })
    .Build();

host.Run();
