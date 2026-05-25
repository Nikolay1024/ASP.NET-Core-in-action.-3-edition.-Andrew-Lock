//  Все атрибуты [From*] находятся в этом пространстве имен.
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using HttpJson = Microsoft.AspNetCore.Http.Json;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Можно настроить System.Text.Json, чтобы ослабить ограничения и контролировать сериализацию.
builder.Services.Configure<HttpJson.JsonOptions>(options =>
{
    // Разрешает конечные запятые в записи свойств JSON.
    options.SerializerOptions.AllowTrailingCommas = true;
    // Устанавливает нечувствительность к регистру имен свойств JSON.
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
});
WebApplication app = builder.Build();

// GET /products/123/paged?page=1 HTTP/1.1
// Host: localhost
// PageSize: 20

// [FromRoute] привязывает аргумент к параметру маршрута. Привязка выполняется неявно по умолчанию.
// [FromQuery] привязывает аргумент к строке запроса. Привязка выполняется неявно, если аргумент не найден
// в параметрах маршрута.
// [FromHeader] привязывает аргумент к указанному заголовку.
app.MapGet("/example1/products/{id}/paged", ([FromRoute] int id, [FromQuery] int page,
    [FromHeader(Name = "PageSize")] int pageSize) =>
        $"Получено id {id}, page {page}, pageSize {pageSize}.");

// Аргумент id неявно привязывается к параметру маршрута, поскольку тип ProductId является простым.
app.MapGet("/example2/product/{id}", (ProductId id) => $"Получено {id}.");

// Аргумент product неявно привязывается к телу запроса в формате JSON, поскольку тип Product является сложным. 
app.MapPost("/example3/product", (Product product) => $"Получено {product}.");

// /products/search?id=123&id=456
// Аргумент id (массив) привязывается к нескольким параметрам id в строке запроса.
// В примере мы привязываем int[], но можно привязать массив любого простого типа.
app.MapGet("/example4/products/search1", (int[] id) => $"Получено {string.Join(", ", id)}.");
app.MapGet("/example4/products/search2", ([FromQuery(Name = "id")] int[] ids) => $"Получено {string.Join(", ", ids)}.");

// Необязательный аргумент int? id привязывается к необязательному параметру маршрута {id?}.
app.MapGet("/example5/stock1/{id?}", (int? id) => $"Получено {id?.ToString() ?? "null"}.");
// Необязательный аргумент int? id привязывается к необязательной строке запроса ?id=123.
app.MapGet("/example5/stock2", (int? id) => $"Получено {id?.ToString() ?? "null"}.");
// Необязательный аргумент Product? product привязывается к необязательному телу запроса в формате JSON.
app.MapPost("/example5/stock3", (Product? product) => $"Получено {product?.ToString() ?? "null"}.");

// Сервис LinkGenerator можно использовать в качестве параметра, поскольку он доступен в контейнере внедрения
// зависимостей. Атрубут [FromServices] в данном примере можно опустить, он будет применен неявно.
app.MapPost("/example6/products", () => "Товары.").WithName("products");
app.MapGet("/example6/links", ([FromServices] LinkGenerator links) =>
{
    string? link = links.GetPathByName("products");
    return $"Путь к конечной точке {link}.";
});

// Для параметра SizeDetails не требуются дополнительные атрибуты, поскольку тип реализует метод BindAsync().
// В методе BindAsync() читается тело запроса, и на основе разобранных строк создается объект SizeDetails.
app.MapPost("/example7/sizes", (SizeDetails size) => $"Получено {size}.");

// Атрибут [AsParameters] позволяет объединить все аргументы в один тип, упрощая сигнатуру метода обработчика.
app.MapGet("/example8/category1/{id}", (int id, int page, [FromHeader(Name = "sort")] bool? sortAsc,
    [FromQuery(Name = "q")] string search) =>
        $"Получено id {id}, page {page}, sortAsc {sortAsc}, search {search}.");
app.MapGet("/example8/category2/{id}", ([AsParameters] SearchModel model) => $"Получено {model}.");

app.Run();


// ProductId – это record struct, которые появились в  C# 10.
// ProductId реализует метод TryParse(), поэтому это простой тип.
readonly record struct ProductId(int Id)
{
    public static bool TryParse(string? s, out ProductId result)
    {
        if (s is not null && s.StartsWith('p') && int.TryParse(s.AsSpan().Slice(1), out int id))
        {
            result = new ProductId(id);
            return true;
        }

        result = default;
        return false;
    }
}

// Product не реализует метод TryParse(), поэтому это сложный тип.
record Product(int Id, string Name, int Stock);

public record SizeDetails(double Height, double Width)
{
    // Тип SizeDetails реализует статический метод BindAsync().
    public static async ValueTask<SizeDetails?> BindAsync(HttpContext httpContext)
    {
        // Создает StreamReader для чтения тела запроса.
        using var streamReader = new StreamReader(httpContext.Request.Body);

        string? line1 = await streamReader.ReadLineAsync(httpContext.RequestAborted);
        if (line1 is null)
            return null;
        string? line2 = await streamReader.ReadLineAsync(httpContext.RequestAborted);
        if (line2 is null)
            return null;

        return double.TryParse(line1, out double height) && double.TryParse(line2, out double width) ?
            new SizeDetails(height, width) : null;
    }
}

// Каждый параметр привязан так, как если бы он был записан в обработчике конечной точки.
record struct SearchModel(int Id, int Page, [FromHeader(Name = "sort")] bool? SortAsc,
    [FromQuery(Name = "q")] string Search);