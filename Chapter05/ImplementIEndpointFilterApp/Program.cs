using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit/{id}", (string id) =>
        fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404))
    // Добавляет фильтр с помощью обобщенного метода AddEndpointFilter.
    .AddEndpointFilter<IdValidationFilter>();

app.Run();


record Fruit(string Name, int Stock);

// Фильтр должен реализовать IEndpointFilter, который требует реализации метода InvokeAsync().
class IdValidationFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        string id = context.GetArgument<string>(0);
        if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
        {
            return Results.ValidationProblem(new Dictionary<string, string[]>()
            {
                { "id", new string[] { "Неверный формат. Идентификатор должен начинаться с 'f'." } }
            });
        }
        return await next(context);
    }
}
