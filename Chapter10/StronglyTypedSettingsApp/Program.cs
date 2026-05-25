using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Привязка секций конфигурации AppDisplaySettings к классу параметров конфигурации AppDisplaySettings.
builder.Services.Configure<AppDisplaySettings>(builder.Configuration.GetSection(nameof(AppDisplaySettings)));
builder.Services.Configure<MapSettings>(builder.Configuration.GetSection(nameof(MapSettings)));
builder.Services.Configure<List<Store>>(builder.Configuration.GetSection("Stores"));

// Задаёт для JSON формат с отступами. По умолчанию JSON сериализуется без отступов.
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);

WebApplication app = builder.Build();

// Рабочий, но не сторого типизированный вариант внедрения параметров конфигурации.
app.MapGet("/display-settings1", (IConfiguration config) =>
{
    string title = config["AppDisplaySettings:Title"] ?? "null";
    if (!bool.TryParse(config["AppDisplaySettings:ShowCopyright"], out bool showCopyright))
        showCopyright = false;
    return new { title, showCopyright, };
});

// Внедрение строго типизированных неперезагружаемых параметров конфигурации с помощью связвателя IOptions<T>.
// Объект IOptions<T> создается в момент внедрения зависимости и имеет жизненный цикл Singleton.
app.MapGet("/display-settings2", (IOptions<AppDisplaySettings> options) =>
{
    // Свойство Value предоставляет объект параметров конфигурации.
    AppDisplaySettings settings = options.Value;
    return new { title = settings.Title, showCopyright = settings.ShowCopyright, };
});

// Внедрение перезагружаемых параметров конфигурации с помощью связвателя IOptionsSnapshot<T>.
// Объект IOptionsSnapshot<T> создается в момент внедрения зависимости и имеет жизненный цикл Scoped.
// IOptionsSnapshot<T> обновляется при новом запросе, если соответствующие значения конфигурации изменяются.
app.MapGet("/map-settings", (IOptionsSnapshot<MapSettings> options) => options.Value);
app.MapGet("/stores", (IOptionsSnapshot<List<Store>> options) => options.Value);

app.Run();


// Классы параметров конфигурации. Эти классы не должны быть абстрактными и должны иметь конструктор
// public без параметров, чтобы привязка работала корректно. Связыватель установит все открытые свойства,
// совпадающие с параметрами конфигурации.
class AppDisplaySettings
{
    public string? Title { get; set; }
    public bool ShowCopyright { get; set; }
}
class Location
{
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
}
class MapSettings
{
    public int DefaultZoomLevel { get; set; }
    public Location? DefaultLocation { get; set; }
}
class Store
{
    public string? Name { get; set; }
    public Location? Location { get; set; }
}
