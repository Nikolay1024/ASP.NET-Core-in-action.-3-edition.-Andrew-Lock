using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit/{id}", (string id) =>
{
    if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
    {
        return Results.ValidationProblem(new Dictionary<string, string[]>()
        {
            { "id", new string[] { "Неверный формат. Идентификатор должен начинаться с 'f'." } }
        });
    }
    return fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) :
        Results.Problem(statusCode: 404);
});

app.Run();


record Fruit(string Name, int Stock);
