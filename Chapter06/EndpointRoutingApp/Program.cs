WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Добавляет сервисы, необходимые для компонента проверки работоспособности и Razor Pages.
builder.Services.AddHealthChecks();
builder.Services.AddRazorPages();
WebApplication app = builder.Build();

// Регистрирует конечную точку минимального API, которая возвращает «Hello World!» на маршруте /test.
app.MapGet("/test", () => "Hello World!");
// Регистрирует конечную точку проверки работоспособности на маршруте /healthz.
app.MapHealthChecks("/healthz");
// Регистрирует все страницы Razor Pages в приложении в качестве конечных точек.
app.MapRazorPages();

// Компонент маршрутизации RoutingMiddleware сопостовляет URL входящего запроса с "шаблонами маршрута" и
// определяет конечную точку, которая будет выполнена компонентом EndpointMiddleware.
// Шаблон маршрута разделен на сегменты символом '/'. Каждый сегмент может быть представлен литералом /product,
// обязательным параметром /{category}, необязательным параметром со значением по умолчанию /{name=all},
// необязательным параметром /{id?}. Однако, если есть необязательный параметр, он должен быть в конце шаблона.
app.MapGet("/example1/product/{category}/{name=all}/{id?}", (string category, string name, int? id) =>
    $"Продукт из категории {category} с названием {name} имеет идентификатор {id.GetValueOrDefault()}.");

// Чтобы избежать неоднозначного сопоставления URL входящего запроса с шаблонами маршрута можно использовать
// "ограничения". Не надо использовать ограничения для валидации пользовательских данных.
// Можно комбинировать ограничения, разделяя их двоеточием.
app.MapGet("/example2/product/{id:int}", (int id) => $"Идентификатор продукта {id}.");
app.MapGet("/example2/{name:alpha:length(8,16)}", (string name) => $"Название продукта {name}.");

// Для сопоставления URL с произвольным кол-вом сегментов используется "универсальный параметр" /{**others}.
app.MapGet("/example3/{currency}/convert/{**others}", (string currency, string? others) =>
{
    string destCurrency = string.IsNullOrEmpty(others) ? "null" : string.Join(", ", others.Split('/'));
    return $"Показать курс обмена валюты {currency} на {destCurrency}.";
});

app.MapGet("/example4/product/{name}", (string name) => $"Название продукта {name}.")
    .WithName("product"); // Дает конечной точке имя, добавляя к ней метаданные.
app.MapGet("/example4/links", (LinkGenerator links) =>
{
    // Создает путь, используя имя конечной точки product, и предоставляет значение для параметра маршрута.
    string? path = links.GetPathByName("product", new { name = "big-widget" });
    // Создает ссылку.
    string? link = links.GetUriByName("product", new { name = "super-fancy-widget" },
        "https", new HostString("localhost"));
    return $"Путь к конечной точке {path}.\nСсылка на конечную точку {link}.";
});

app.MapGet("/example5/hello", () => "Hello world!").WithName("hello");
// Генерирует ответ с кодом 302, который отправляет перенаправление в конечную точку hello.
app.MapGet("/example5/redirect-to-route", () => Results.RedirectToRoute("hello"));
app.MapGet("/example5/redirect-to-url", () => Results.Redirect("https://ya.ru"));

app.Run();
