using Microsoft.AspNetCore.HttpLogging;

// В .NET 7 все шаблоны по умолчанию используют инструкции верхнего уровня.
// Компилятор генерирует класс Program с методом Main() и помещает инструкции в метод Main().
// Переменная args все также хранит параметры запуска приложения.
// Можно явно использовать класс Program с методом Main() это все также будет работать.
// Т.к. файл с инструкциями верхнего уровня является точкой входа приложения,
// он может быть только один в проекте.

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Регистрация сервиса в IoC-контейнере (Inversion of Control).
// Регистрация сервиса в контейнере внедрение зависимостей DI (Dependency Injection).
builder.Services.AddHttpLogging(opts => opts.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);
WebApplication app = builder.Build();

// WebApplication неявно добавляет компоненты (промежуточное ПО, middleware) в конвейер обработки HTTP-запроса.
// Когда приложение работает в окружении разработки в начале добавляется компонент
// страницы исключений разработчика (DeveloperExceptionPageMiddleware).
// Следом добавляется компонент маршрутизации (RoutingMiddleware).

if (app.Environment.IsDevelopment())
    // Добавление компонента журналирования HTTP-запросов.
    app.UseHttpLogging();

// WebApplication неявно добавляет компоненты (промежуточное ПО, middleware) в конвейер обработки HTTP-запроса.
// В конце добавляется компонент конечной точки (EndpointMiddleware).

// Регистрация конечных точек.
app.MapGet("/", () => "Hello World!");
app.MapGet("/person", () => new Person("Andrew", "Lock"));

app.Run();

public record Person(string FirstName, string LastName);
