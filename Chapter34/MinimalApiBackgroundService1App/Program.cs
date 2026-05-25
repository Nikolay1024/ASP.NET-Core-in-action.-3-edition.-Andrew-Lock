using Microsoft.Extensions.Options;
using MinimalApiBackgroundService1App.BackgroundServices;
using MinimalApiBackgroundService1App.Cache;
using MinimalApiBackgroundService1App.HttpClients;
using MinimalApiBackgroundService1App.Options;
using Polly;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Задаёт для JSON формат с отступами. По умолчанию JSON сериализуется без отступов.
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);
// Настраивает объект OptionsBuilder<T> путем привязки к разделу "RemoteApi" IConfiguration.
builder.Services.AddOptions<RemoteApiOptions>().BindConfiguration("RemoteApi");

// Регистрация сервиса типизированного HTTP клиента.
IHttpClientBuilder httpClientBuilder = builder.Services.AddHttpClient<RemoteApiHttpClient>(
    (IServiceProvider serviceProvider, HttpClient httpClient) =>
    {
        IOptions<RemoteApiOptions> options = serviceProvider.GetRequiredService<IOptions<RemoteApiOptions>>();
        httpClient.BaseAddress = new Uri(options.Value.Url);
    });
httpClientBuilder.AddTransientHttpErrorPolicy((PolicyBuilder<HttpResponseMessage> policy) =>
    policy.WaitAndRetryAsync(3, (int retryNumber) => TimeSpan.FromSeconds(retryNumber)));

// Регистрация сервиса кеша с жизненным циклом Singleton, т.к. используется один экземпляр во всем приложении.
builder.Services.AddSingleton<RemoteApiCache>();
// Регистрация фонового сервиса RemoteApiBackgroundService как IHostedService.
builder.Services.AddHostedService<RemoteApiBackgroundService>();

WebApplication app = builder.Build();

app.MapGet("/cache", (RemoteApiCache cache) => cache.Albums);
app.MapGet("/http-client", async (RemoteApiHttpClient httpClient) => await httpClient.GetAlbumsAsync());

app.Run();
