using Microsoft.Extensions.Options;
using MinimalApiConfigureOptionsApp.Options;
using MinimalApiConfigureOptionsApp.OptionsProviders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Задаёт для JSON формат с отступами. По умолчанию JSON сериализуется без отступов.
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);


#region Настройка с помощью подхода IConfigureOptions<T>.
// Настраивает объект IOptions<T> путем привязки к разделу "Languages" IConfiguration.
builder.Services.Configure<LanguageOptions>(builder.Configuration.GetSection("Languages"));
// Настраивает объект IOptions<T>, выполняя лямбда-выражение.
builder.Services.Configure((LanguageOptions options) =>
    options.SupportedLanguages = new string[] { "English", "French", "Spanish" });

// Регистрация сервиса в контейнере внедрения зависимостей.
// Место регистрации определяет порядок, в котором объект IOptions<T> будет настраиваться. Поэтому,
// чтобы ConfigureLanguageOptions.Configure() запускался в конце, вы должны зарегистрировать сервис здесь.
builder.Services.AddSingleton<IConfigureOptions<LanguageOptions>, ConfigureLanguageOptions>();
// Регистрация сервиса ILanguageOptionsProvider, который является зависимостью IConfigureOptions<T>. 
// В сервис IConfigureOptions<T> возможно внедрять сервисы только с жизненным циклом Singleton.
builder.Services.AddSingleton<ILanguageOptionsProvider, LanguageOptionsProvider>();
#endregion


#region Настройка с помощью подхода OptionsBuilder<T>.
// Создает объект OptionsBuilder<T>.
OptionsBuilder<CurrencyOptions> optionsBuilder = builder.Services.AddOptions<CurrencyOptions>();
// Настраивает объект OptionsBuilder<T> путем привязки к разделу "Currencies" IConfiguration.
optionsBuilder.BindConfiguration("Currencies");
// Настраивает объект OptionsBuilder<T>, выполняя лямбда-выражение.
optionsBuilder.Configure((CurrencyOptions options) =>
    options.SupportedСurrencies = new string[] { "GBP", "USD", "EUR" });
// Настраивает объект OptionsBuilder<T> с помощью сервиса из контейнера внедрения зависимостей.
// С помощью методов Configure<TDep>() возможно внедрять сервисы только с жизненным циклом Singleton.
// Если вы попытаетесь внедрить сервис Scoped, например DbContext, вы получите сообщение об ошибке
// захваченной зависимости (captured dependency).
optionsBuilder.Configure((CurrencyOptions options, ICurrencyOptionsProvider optionsProvider) =>
    options.SupportedСurrencies = optionsProvider.GetSupportedCurrencies());

// Регистрация сервиса в контейнере внедрения зависимостей.
// С помощью методов Configure<TDep>() возможно внедрять сервисы только с жизненным циклом Singleton.
builder.Services.AddSingleton<ICurrencyOptionsProvider, CurrencyOptionsProvider>();
#endregion


#region Внедрение зависимости с жизненным циклом Scoped в OptionsBuilder<T>.Configure().
builder.Services.AddOptions<MyOptions>()
    .Configure((MyOptions options, IServiceProvider serviceProvider) =>
    {
        using IServiceScope scope = serviceProvider.CreateScope();
        GuidService guidService = scope.ServiceProvider.GetRequiredService<GuidService>();
        options.Guid = guidService.GetGuid();
    });

builder.Services.AddScoped<GuidService>();
#endregion


#region Валидация настроек.
builder.Services.AddOptions<SlackApiOptions>().BindConfiguration("SlackApi")
    // Включение валидации настроек.
    .ValidateDataAnnotations()
    // Установка валидации настроек при запуске приложения.
    .ValidateOnStart();
#endregion


WebApplication app = builder.Build();

// Внедренное значение IOptions<LanguageOptions> создается путем привязки к конфигурации и
// применения лямбда-выражения.
// Внедренное значение IOptions<CurrencyOptions> создается путем привязки к конфигурации,
// применения лямбда-выражения и использования сервиса из контейнера внедрения зависимостей.
app.MapGet("/", (IOptions<LanguageOptions> languages, IOptions<CurrencyOptions> currencies,
    IOptions<MyOptions> myOptions, IOptions<SlackApiOptions> slackApi) =>
        new { Languages = languages.Value, Currencies = currencies.Value, myOptions.Value.Guid,
            SlackApi = slackApi.Value });

app.Run();
