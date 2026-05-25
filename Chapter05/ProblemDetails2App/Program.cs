using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Регистрирует сервис IProblemDetailsService для преобразования исключений в коды ошибок HTTP с телом ответа
// Problem Details. Т.е. при перехвате исключений компонентами ковейера ExceptionHandlerMiddleware и
// DeveloperExceptionPageMiddleware. Также требуется для работы компонента StatusCodePagesMiddleware.
builder.Services.AddProblemDetails();
WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler();
// Добавляет компонент StatusCodePagesMiddleware, который преобразует ответы с кодом ошибки HTTP
// в формат Problem Details. Каждый ответ с кодом ошибки и без тела, когда достигает компонента,
// получает тело ответа Problem Details.
app.UseStatusCodePages();

ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit", () => fruits);
// Компонент StatusCodePagesMiddleware добавляет тело Problem Details в ответ с кодом ошибки HTTP 404.
app.MapGet("/fruit/{id}", (string id) =>
    fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.NotFound());
// Компонент StatusCodePagesMiddleware добавляет тело Problem Details в ответ с кодом ошибки HTTP 400.
app.MapPost("/fruit/{id}", (string id, Fruit fruit) =>
    fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) : Results.BadRequest());
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
