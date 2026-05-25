WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Поставщики по умолчанию: поставщик файлов JSON, пользовательские секреты (User Secrets),
// переменные окружения, аргументы командной строки.
// Очищает поставщиков, настроенных по умолчанию в WebApplicationBuilder.
builder.Configuration.Sources.Clear();

// Добавление поставщика кофигурации для файла JSON.
builder.Configuration.AddJsonFile("appsettings.json");
// Добавленные позднее поставщики могут переопределять конечный файл конфигурации IConfiguration.
// Здесь переопределяется ключ MyAppConnString.
// Второй параметр optional:true указывает, что в случае отсутствия файла не будет вызвано исключение.
// Третий параметр reloadOnChange:true указывает, что IConfiguration будет собран снова, если файл изменится.
builder.Configuration.AddJsonFile("production-settings.json", true, true);

// Добавление поставщика кофигурации для пользовательских секретов secrets.json.
// Следует использовать в среде разработки для строк подключения и API ключей.
// Поставщик кофигурации для пользовательских секретов должен прочитать свойство UserSecretsId, которое вы
// (или Visual Studio) добавили в файл с расширением .csproj.
if (builder.Environment.IsDevelopment())
    builder.Configuration.AddUserSecrets<Program>();

// Добавление поставщика кофигурации для переменных окружения.
// Следует использовать в промышленной среде для строк подключения и API ключей.
if (!builder.Environment.IsProduction())
    builder.Configuration.AddEnvironmentVariables();

// Задаёт для JSON формат с отступами. По умолчанию JSON сериализуется без отступов.
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);

WebApplication app = builder.Build();

// Возвращает все пары конфигурации "ключ–значение" для отображения.
//app.MapGet("/", () => app.Configuration.AsEnumerable());

// ConfigurationManager также регистрируется как IConfiguration в контейнере внедрения зависимостей (DI),
// поэтому вы можете внедрить его в обработчик конечной точки.
app.MapGet("/", (IConfiguration config) => config.AsEnumerable());

app.MapGet("/default-zoom-level", (IConfiguration config) =>
    $"DefaultZoomLevel: {config["MapSettings:DefaultZoomLevel"]}");
app.MapGet("/latitude", (IConfiguration config) =>
    $"Latitude: {config["MapSettings:DefaultLocation:Latitude"]}");

app.Run();
