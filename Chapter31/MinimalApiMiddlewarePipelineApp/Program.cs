using MinimalApiMiddlewarePipelineApp.Middlewares;
using System.Net.Mime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

// Каждый запрос будет проходить через это промежуточное ПО.
app.UseExceptionHandler();
app.UseStatusCodePages();

// Метод Run() создает промежуточное ПО, которое возвращает ответ, прерывая выполнение конвейера.
//app.Run(async (HttpContext context) => await context.Response.WriteAsync(DateTime.Now.ToString()));

// Метод Map() создает ветвь в конвейере промежуточного ПО.
app.Map("/ping1", (IApplicationBuilder appBranch) =>
{
    // Метод Run() создает промежуточное ПО, которое возвращает ответ, но только в ветке /ping1.
    appBranch.Run(async (HttpContext context) =>
    {
        // Установка заголовка Content-Type и статусного кода для генерируемого ответа.
        // text/plain, 200 - значения по умолчанию.
        context.Response.ContentType = MediaTypeNames.Text.Plain;
        context.Response.StatusCode = StatusCodes.Status200OK;

        // Генерирует ответ и прерывает выполнение ветки /ping1 конвейера.
        await context.Response.WriteAsync("pong1");
    });
});

// Метод Use() создает промежуточное ПО, которое решает возвратить ответ и/или продолжить выполнение конвейера.
app.Use(async (HttpContext context, Func<Task> next) =>
{
    // Метод StartsWithSegments() ищет сегмент /ping2 в пути запроса.
    if (context.Request.Path.StartsWithSegments("/ping2"))
        // Если сегмент найден, генерирует ответ и прерывает выполнение конвейера.
        await context.Response.WriteAsync("pong2");
    else
        // Если сегмент не найден, вызывает следующее промежуточное ПО в конвейере.
        await next();
});

app.Map("/branch1", (IApplicationBuilder appBranch) =>
{
    // Использование метода Use() для добавления заголовка безопасности к ответу.
    appBranch.Use(async (HttpContext context, Func<Task> next) =>
    {
        // Задает лямбда-выражение, которое должно вызываться перед отправкой ответа в браузер.
        context.Response.OnStarting(() =>
        {
            // Добавляет заголовок в ответ для дополнительной защиты от XSS атак.
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            // Лямбда-выражение, переданное в метод OnStarting(), должно возвращать Task.
            return Task.CompletedTask;
        });

        // Вызывает следующее промежуточное ПО в конвейере.
        await next();
    });
});

app.Map("/branch2", (IApplicationBuilder appBranch) =>
{
    // Метод UseMiddleware<T>() добавляет промежуточного ПО T в конвейер.
    appBranch.UseMiddleware<SecurityHeadersMiddleware>();
    appBranch.UseMiddleware<VersionMiddleware>();
});

app.Map("/branch3", (IApplicationBuilder appBranch) =>
{
    // Методы расширения типа IApplicationBuilder, добавляющие промежуточное ПО в конвейер.
    appBranch.UseSecurityHeadersMiddleware();
    appBranch.UsePingPong3Middleware();
    appBranch.UseSecurityTxtMiddleware();
});


// Конечные точки (Endpoints).
app.MapFallback(() => """
    Попробуйте один из следующих маршрутов Map/Use/Run:
    /ping1
    /ping2
    /branch1 (Проверьте заголовок ответа на наличие X-Content-Type-Options:nosniff)
    /branch2
    /branch3/ping3
    /branch3/security.txt
    
    Или попробуйте один из следующих маршрутов к конечной точке:
    /branch4/ping4
    /branch5/ping4
    /branch5/version
    /add/a/b
    /add/1/2
    """);

// Приводим WebApplication к IEndpointRouteBuilder, чтобы можно было вызвать метод CreateApplicationBuilder().
var endpoints = (IEndpointRouteBuilder)app;
// Создаем конвейер промежуточного ПО для обработки конечной точки.
IApplicationBuilder piplineBuilder = endpoints.CreateApplicationBuilder();
// Добавляем промежуточное ПО в конвейер.
piplineBuilder = piplineBuilder.UseMiddleware<PingPong4Middleware>();
// Собираем конвейер из добавленного промежуточного ПО.
// Выполнение конвейера промежуточного ПО конечной точки происходит при вызове конечной точки.
RequestDelegate pipline = piplineBuilder.Build();

// Создаем новую конечную точку, сопоставляя шаблоном маршрута /ping4 с конвейером промежуточного ПО.
app.Map("/branch4/ping4", pipline);

// Методы расширения типа IEndpointRouteBuilder, добавляющие промежуточное ПО в качестве конечной точки.
// Вы можете добавить метаданные в методе расширения и/или в файле Program.cs.
app.MapPingPong4("/branch5/ping4");
app.MapVersion("/branch5/version").WithName("Version");

// Пример использования параметров маршрута для конечной точки.
app.MapMiddlewareAsEndpoint<CalculatorMiddleware>("/add/{a?}/{b?}");

app.Run();
