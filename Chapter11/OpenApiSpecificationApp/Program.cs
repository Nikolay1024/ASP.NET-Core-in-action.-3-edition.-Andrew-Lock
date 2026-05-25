using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Collections.Concurrent;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Добавляет функции обнаружения конечных точек ASP.NET Core, необходимые Swashbuckle.
builder.Services.AddEndpointsApiExplorer();
// Добавляет службы Swashbuckle, необходимые для создания документов OpenAPI.
// builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen((SwaggerGenOptions options) =>
{
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "Фруктовый склад.",
        Description = "API для взаимодействия с фруктовым складом.",
        Version = "1.0"
    });
    // Включает комментарии в формате XML для описаний OpenAPI.
    string xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    string xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
    options.IncludeXmlComments(xmlFilePath);
});

WebApplication app = builder.Build();

var fruits = new ConcurrentDictionary<string, Fruit>();

// Добавляет компонент для предоставления документа OpenAPI для вашего приложения.
// OpenAPI (ранее известная как Swagger) – это независимая от языка спецификация для описания REST API в
// формате JSON.
app.UseSwagger();
// Добавляет компонент, обслуживающий Swagger UI.
// Swagger UI - пользовательский интерфейс инструмента анализа и тестирования REST API.
app.UseSwaggerUI((SwaggerUIOptions options) =>
{
    // Устанавливает путь конечной точки для Swagger UI / вместо /swagger.
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1"); 
    options.RoutePrefix = string.Empty;
});

// /swagger/v1/swagger.json - документ OpenAPI для вашего приложения.
// /swagger - пользовательский интерфейс инструмента Swagger UI.

// WithTags("fruit") - группирует конечные точки в Swagger UI. Конечная точка может иметь несколько тегов.
// Produces<Fruit>() - конечная точка может возвращать объект Fruit. Если не указано, предполагается ответ 200.
// Produces<Fruit>(201) - конечная точка также возвращает объект Fruit, но использует ответ 201.
// ProducesProblem(404) - если идентификатор не найден, конечная точка возвращает ответ 404.
// ProducesValidationProblem() - если идентификатор уже существует, конечная точка возвращает ответ 400.

// WithSummary() - добавляет описание в конечную точку.
// WithDescription() - добавляет сводку в конечную точку.
// WithOpenApi() - предоставляет метаданные, добавленные посредством сводки и описания, в описание OpenAPI.
app.MapGet("/fruit1/{id}", (string id) =>
    fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404))
    .WithName("GetFruit1").WithTags("Fruits1").Produces<Fruit>().ProducesProblem(404)
    .WithSummary("Получает фрукт.").WithDescription("Получает фрукт по идентификатору или возвращает 404, " +
    "если фрукта с указанным идентификатором не существует.")
    .WithOpenApi((OpenApiOperation operation) =>
    {
        operation.Parameters[0].Description = "Идентификатор фрукта, который нужно получить.";
        return operation;
    });

app.MapPost("/fruit1/{id}", (string id, Fruit fruit) =>
    fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) :
        Results.ValidationProblem(new Dictionary<string, string[]>()
        {
            { "id", new string[] { "Фрукт с таким идентификатором уже существует." } }
        }))
    .WithName("PostFruit1").WithTags("Fruits1").Produces<Fruit>(201).ProducesValidationProblem()
    .WithSummary("Создает фрукт.").WithDescription("Создаёт фрукт с указанным идентификатором. Фрукт не " +
    "должен существовать.")
    .WithOpenApi((OpenApiOperation operation) =>
    {
        operation.Parameters[0].Description = "Идентификатор создаваемого фрукта.";
        return operation;
    });


app.MapGet("/fruit2/{id}",
    [Tags("Fruits2"), EndpointName("GetFruit2"), EndpointSummary("Получает фрукт.")]
    [EndpointDescription("Получает фрукт по идентификатору или возвращает 404, если фрукта с указанным " +
    "идентификатором не существует.")]
    [ProducesResponseType(typeof(Fruit), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404, "application/problem+json")]
    (string id) =>
    fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404))
    .WithOpenApi((OpenApiOperation operation) =>
    {
        operation.Parameters[0].Description = "Идентификатор фрукта, который нужно получить.";
        return operation;
    });

app.MapPost("/fruit2/{id}",
    [Tags("Fruits2"), EndpointName("PostFruit2"), EndpointSummary("Создает фрукт.")]
    [EndpointDescription("Создаёт фрукт с указанным идентификатором. Фрукт не должен существовать.")]
    [ProducesResponseType(typeof(Fruit), 201)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), 400, "application/problem+json")]
    (string id, Fruit fruit) =>
    fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) :
        Results.ValidationProblem(new Dictionary<string, string[]>()
        {
            { "id", new string[] { "Фрукт с таким идентификатором уже существует." } }
        }))
    .WithOpenApi((OpenApiOperation operation) =>
    {
        operation.Parameters[0].Description = "Идентификатор создаваемого фрукта.";
        return operation;
    });


var fruitHandler = new FruitHandler(fruits);
app.MapGet("/fruit3/{id}", fruitHandler.GetFruit).WithName("GetFruit3");
app.MapPost("/fruit3/{id}", fruitHandler.PostFruit).WithName("PostFruit3");

app.Run();


record Fruit(string Name, int Stock);

class FruitHandler
{
    readonly ConcurrentDictionary<string, Fruit> Fruits;
    
    public FruitHandler(ConcurrentDictionary<string, Fruit> fruits) => Fruits = fruits;

    /// <summary>
    /// Получает фрукт.
    /// </summary>
    /// <param name="id" >Идентификатор фрукта, который нужно получить.</param>
    /// <response code="200">Возвращает фрукт, если он существует.</response>
    /// <response code="404">Возвращает 404, если фрукт не существует.</response>
    [Tags("Fruits3")]
    [ProducesResponseType(typeof(Fruit), 200)]
    [ProducesResponseType(typeof(ProblemDetails), 404, "application/problem+json")]
    public IResult GetFruit(string id) =>
        Fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404);

    /// <summary>
    /// Создает фрукт.
    /// </summary>
    /// <param name="id" >Идентификатор создаваемого фрукта.</param>
    /// <param name="fruit" >Фрукт, который необходимо создать.</param>
    /// <response code="201">Возвращает фрукт, который был создан.</response>
    /// <response code="400">Возвращает 400, если фрукт с таким идентификатором уже существует.</response>
    [Tags("Fruits3")]
    [ProducesResponseType(typeof(Fruit), 201)]
    [ProducesResponseType(typeof(HttpValidationProblemDetails), 400, "application/problem+json")]
    public IResult PostFruit(string id, Fruit fruit) =>
        Fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) :
            Results.ValidationProblem(new Dictionary<string, string[]>()
            {
                { "id", new string[] { "Фрукт с таким идентификатором уже существует." } }
            });
}
