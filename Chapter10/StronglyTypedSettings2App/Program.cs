using Microsoft.Extensions.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Настройка строго типизированных параметров конфигурации без интерфейса IOptions<T>.
var appDisplaySettings1 = new AppDisplaySettings();
builder.Configuration.GetSection(nameof(AppDisplaySettings)).Bind(appDisplaySettings1);
builder.Services.AddSingleton(appDisplaySettings1);

// Настройка строго типизированных параметров для прямого внедрения.
builder.Services.Configure<MapSettings>(builder.Configuration.GetSection(nameof(MapSettings)));
builder.Services.AddSingleton(provider => provider.GetRequiredService<IOptions<MapSettings>>().Value);

// Задаёт для JSON формат с отступами. По умолчанию JSON сериализуется без отступов.
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);

WebApplication app = builder.Build();

app.MapGet("/display-settings", (AppDisplaySettings appDisplaySettings2) => appDisplaySettings2);
app.MapGet("/map-settings", (MapSettings mapSettings) => mapSettings);

app.Run();


// Классы параметров конфигурации.
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
