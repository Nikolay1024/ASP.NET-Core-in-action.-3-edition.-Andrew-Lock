using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using MinimalApiCallingRemoteApiApp.HttpClients;
using MinimalApiCallingRemoteApiApp.HttpMessageHandlers;
using MinimalApiCallingRemoteApiApp.Options;
using Polly;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Mime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen((SwaggerGenOptions options) =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "JsonPlaceholder.",
        Description = "API для взаимодействия с удаленным API JsonPlaceholder.",
        Version = "1.0"
    });
});


// Регистрация сервиса IHttpClientFactory (Transient).
//builder.Services.AddHttpClient();


// Регистрация сервиса IHttpClientFactory (Transient), который является именованным HTTP клиентом.
// Указываем имя HTTP клиента и лямбда-выражение конфигурации.
// Лямбда-выражение конфигурации выполняется каждый раз, когда запрашивается именованный HTTP клиент.
IHttpClientBuilder httpClientBuilder = builder.Services.AddHttpClient("RemoteApi",
    (IServiceProvider serviceProvider, HttpClient httpClient) =>
    {
        IOptions<RemoteApiOptions> options = serviceProvider.GetRequiredService<IOptions<RemoteApiOptions>>();
        httpClient.BaseAddress = new Uri(options.Value.Url);
    });
// Вы можете добавить лямбда-выражения конфигурации для именованного HTTP клиента, которые будут выполняться
// последовательно. Существуют дополнительные перегруженные варианты, которые разрешают доступ к контейнеру
// внедрения зависимостей при создании именованного HTTP клиента.
httpClientBuilder.ConfigureHttpClient((IServiceProvider serviceProvider, HttpClient httpClient) => { });


// Регистрация сервиса RemoteApiHttpClient (Transient), который является типизированным HTTP клиентом.
// Регистрирует типизированного HTTP клиента, используя обобщенный метод AddHttpClient<T>().
// Можно конфигурировать типизированный HTTP клиент либо в файле Program.cs, либо в конструкторе
// типизированного HTTP клиента.
httpClientBuilder = builder.Services.AddHttpClient<RemoteApiHttpClient>(
    (IServiceProvider serviceProvider, HttpClient httpClient) =>
    {
        IOptions<RemoteApiOptions> options = serviceProvider.GetRequiredService<IOptions<RemoteApiOptions>>();
        httpClient.BaseAddress = new Uri(options.Value.Url);
    });
httpClientBuilder.ConfigureHttpClient((IServiceProvider serviceProvider, HttpClient httpClient) => { });


// Вы можете добавить обработчики в конвейер именованных или типизированных HTTP клиентов.
// Обработчики будут выполняться в том порядке, в котором вы их добавили.
// Добавление пользовательского обработчика HTTP клиента.
// Этот обработчик добавляет заголовок ApiKey для аутентификации удаленного API.
httpClientBuilder.AddHttpMessageHandler<ApiKeyHttpMessageHandler>()
    // Добавление обработчика временных ошибок HTTP клиента с помощью библиотеки Polly.
    .AddTransientHttpErrorPolicy((PolicyBuilder<HttpResponseMessage> policy) =>
        // Настраивает стратегию, которая выжидает и дважды повторяет запросы в случае возникновения ошибки.
        // По умолчанию обработчик будет повторять любой запрос, который выбрасывает HttpRequestException или
        // возвращает статусный код 5XX (ошибка сервера), или возвращает статусный код 408 (тайм-аут).
        policy.WaitAndRetryAsync(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(5), }));

// Регистрация пользовательского обработчика HTTP клиента в контейнере внедрения зависимостей.
builder.Services.AddTransient<ApiKeyHttpMessageHandler>();
// Настраивает объект OptionsBuilder<T> путем привязки к разделу "RemoteApi" IConfiguration.
builder.Services.AddOptions<RemoteApiOptions>().BindConfiguration("RemoteApi");


WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

IOptions<RemoteApiOptions> remoteApiOptions = app.Services.GetRequiredService<IOptions<RemoteApiOptions>>();
var httpClient = new HttpClient() { BaseAddress = new Uri(remoteApiOptions.Value.Url), };

// Использование одного экземпляра HttpClient для предотвращения исчерпания сокетов.
// Данный пример имеет ошибку, связанную с работой DNS. DNS при создании подключения по протоколу HTTP
// сопоставляет доменное имя example.com с IP-адресом 192.168.0.1. Т.к. объект HttpClient существует на протяжении
// жизни приложения, в случае изменении IP-адреса на стороне целевого API приложение продолжит обращаться к
// устаревшему IP-адресу.
app.MapGet("/http-client", async () =>
{
    // Выполняет GET-запрос к тестовому API JsonPlaceholder.
    // Несколько запросов используют один и тот же экземпляр HttpClient.
    HttpResponseMessage hrm = await httpClient.GetAsync("albums/1");
    // Выбрасывает исключение, если запрос не был успешным.
    hrm.EnsureSuccessStatusCode();

    // Возвращает результаты в формате JSON.
    Stream stream = await hrm.Content.ReadAsStreamAsync();
    FileStreamHttpResult fshr = TypedResults.Stream(stream, MediaTypeNames.Application.Json);
    return fshr;
}).WithOpenApi().WithSummary("Отправляет запросы с помощью Singleton HttpClient.");

// Использование IHttpClientFactory для создания объекта HttpClient.
// Данный пример решает проблемы исчерпания сокетов и работы DNS.
// Внедрение сервиса IHttpClientFactory.
app.MapGet("/http-client-factory",
    async (IHttpClientFactory httpClientFactory, IOptions<RemoteApiOptions> options) =>
    {
        HttpClient httpClient = httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri(options.Value.Url);

        HttpResponseMessage hrm = await httpClient.GetAsync("albums/2");
        hrm.EnsureSuccessStatusCode();

        Stream stream = await hrm.Content.ReadAsStreamAsync();
        FileStreamHttpResult fshr = TypedResults.Stream(stream, MediaTypeNames.Application.Json);
        return fshr;
    })
    .WithOpenApi().WithSummary("Отправляет запросы с помощью IHttpClientFactory.");

// Использование IHttpClientFactory для создания именованного HTTP клиента.
// Данный подход позволяет централизованно конфигурировать HTTP клиенты, исключая ошибки при неправильной повторной
// конфигурации.
app.MapGet("/named-http-client", async (IHttpClientFactory httpClientFactory) =>
{
    // Запрос именованного HTTP клиента с именем "RemoteApi".
    HttpClient httpClient = httpClientFactory.CreateClient("RemoteApi");

    HttpResponseMessage hrm = await httpClient.GetAsync("albums/3");
    hrm.EnsureSuccessStatusCode();

    Stream stream = await hrm.Content.ReadAsStreamAsync();
    FileStreamHttpResult fshr = TypedResults.Stream(stream, MediaTypeNames.Application.Json);
    return fshr;
}).WithOpenApi().WithSummary("Отправляет запросы с помощью именованного HTTP клиента.");

// Использование типизированного HTTP клиента.
// Данный подход инкапсулирует логику взаимодействия с удаленным API в отдельный класс, что позволяет потребителю
// HTTP клиента абстрагироваться от внутренних деталей обращения к удаленному API.
app.MapGet("/typed-http-client", async (RemoteApiHttpClient httpClient) => await httpClient.GetAlbum4Async())
    .WithOpenApi().WithSummary("Отправляет запросы с помощью типизированного HTTP клиента.");

app.Run();
