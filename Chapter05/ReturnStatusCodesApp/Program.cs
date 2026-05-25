using System.Collections.Concurrent;
using System.Net.Mime;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit", () => fruits);
app.MapGet("/fruit/{id}", (string id) =>
    fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.NotFound());
app.MapPost("/fruit/{id}", (string id, Fruit fruit) =>
    fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) :
        Results.BadRequest(new { id = "Фрукт с таким идентификатором уже существует." }));
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
app.MapGet("/teapot", (HttpResponse httpResponse) =>
{
    httpResponse.StatusCode = StatusCodes.Status418ImATeapot;
    httpResponse.ContentType = MediaTypeNames.Text.Plain;
    Task result = httpResponse.WriteAsync("Я чайник!");
    return result;
});

app.Run();


record Fruit(string Name, int Stock);
