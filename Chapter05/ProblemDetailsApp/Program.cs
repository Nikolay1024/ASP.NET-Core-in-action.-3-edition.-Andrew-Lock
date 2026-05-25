using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Регистрирует сервис IProblemDetailsService для преобразования исключений в коды ошибок HTTP с телом ответа
// JSON Problem Details. Т.е. при перехвате исключений компонентами ковейера ExceptionHandlerMiddleware и
// DeveloperExceptionPageMiddleware.
builder.Services.AddProblemDetails();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    // Настраивает DeveloperExceptionPageMiddleware без пути, чтобы он использовал IProblemDetailsService.
    app.UseDeveloperExceptionPage();
else
    // Настраивает ExceptionHandlerMiddleware без пути, чтобы он использовал IProblemDetailsService.
    app.UseExceptionHandler();

ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit", () => fruits);
app.MapGet("/fruit/{id}", (string id) =>
    fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404));
app.MapPost("/fruit/{id}", (string id, Fruit fruit) =>
    fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) :
        Results.ValidationProblem(new Dictionary<string, string[]>()
        {
            { "id", new string[] { "Фрукт с таким идентификатором уже существует." } }
        })
);
app.MapPut("/fruit/{id}", (string id, Fruit fruit) =>
{
    fruits[id] = fruit;
    return Results.NoContent();
});
app.MapDelete("/fruit/{id}", (string id) =>
{
    fruits.TryRemove(id, out _);
    return Results.NoContent();
});
app.MapGet("/exception", void () => throw new Exception());

app.Run();


record Fruit(string Name, int Stock);
