using MinimalApiLoggingApp;
using System.Text.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ASP.NET Core неявно добавляет поставщиков логирования для консоли и отладки.
//builder.Logging.AddConsole();
//builder.Logging.AddDebug();

// ASP.NET Core имеет еще два добавляемых поставщика логирования.
// Журнал событий (EventLog) для Windows и EventSource для Windows и Linux.
//if (OperatingSystem.IsWindows())
//    builder.Logging.AddEventLog();
//builder.Logging.AddEventSourceLogger();

// Вызов ClearProviders() удаляет всех добавленных поставщиков логирования.
//builder.Logging.ClearProviders();

// Добавление поставщика логирования для записи журнала в консоль в формате JSON.
builder.Logging.AddJsonConsole(options => options.JsonWriterOptions = new JsonWriterOptions() { Indented = true });
// Добавление поставщика логирования для записи журнала в файл.
builder.Logging.AddFile();
// Добавление поставщика централизованного структурного логирования Seq.
builder.Logging.AddSeq();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ValuesService>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Внедрение сервиса логирования ILogger<T> из контейнера внедрения зависимостей.
// Category. Параметр типа T указывает категорию, полное имя типа, который создал сообщение.
// LogLevel. Уровень логирования указывает важность сообщения (Critical..Trace).
// EventId. Необязательный. Позволяет идентифицировать похожие сообщения журнала. По умолчанию 0.
// Exception. Необязательный. Исключение, которое связано с сообщением журнала.
app.MapGet("/example1", (ILogger<Program> logger) =>
{
    int[] temps = Enumerable.Range(1, 6).Select(i => Random.Shared.Next(-30, 40)).ToArray();

    // Добавление сообщения в журнал. А также пары ключ-значение для поиска и фильтрации сообщений при
    // структурном логировании (MondayWeather - 17).
    // ILogger пишет сообщение во все добавленные поставщики логирования с учетом фильтрации в appsettings.json.

    // Катастрофические ошибки, из-за которых приложение не может работать.
    logger.LogCritical(100, new Exception("Exception massage."), "Critical message. {MondayWeather}", temps[0]);
    // Необработанные ошибки и исключения, которые не влияют на другие запросы.
    logger.LogError(101, new Exception("Exception massage."), "Error message. {TuesdayWeather}", temps[1]);
    // Непредвиденные условия, которые можно обойти, например обработанные исключения.
    logger.LogWarning(102, "Warning message. {WednesdayWeather}", temps[2]);
    // Для отслеживания нормальной работы приложения.
    logger.LogInformation(103, "Info message. {ThursdayWeather}", temps[3]);
    // Для отслеживания подробной информации, особенно во время разработки.
    logger.LogDebug("Debug message. {FridayWeather}", temps[4]);
    // Для получения очень подробной, конфиденциальной информации. Используется редко.
    logger.LogTrace("Trace message. {SaturdayWeather}", temps[5]);

    return temps;
})
.WithTags("Logging");


app.MapGet("/example2", () =>
{
    app.Logger.LogInformation("Перед областью логирования (scope).");

    using (app.Logger.BeginScope(123456))
    using (app.Logger.BeginScope(new Dictionary<string, object> { { "ScopeKey1", "ScopeValue1" } }))
        app.Logger.LogInformation("Внутри области логирования (scope).");

    app.Logger.LogInformation("После области логирования (scope).");

    return Results.Ok("Ok");
})
.WithTags("Logging");

app.MapGet("/example3", Handler.GetValues).WithTags("Logging");

app.Run();
