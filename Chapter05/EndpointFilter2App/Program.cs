using System.Collections.Concurrent;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit/{id}", (string id) =>
        fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404))
    // Добавление фильтра в конечную точку с помощью AddEndpointFilter.
    .AddEndpointFilter(ValidationHelper.ValidateId)
    .AddEndpointFilter(async (context, next) =>
    {
        app.Logger.LogInformation("Выполнение фильтра...");
        // Выполняет оставшуюся часть конвейера и обработчик конечной точки.
        object? result = await next(context);
        // Регистрирует результат, возвращаемый остальной частью конвейера.
        app.Logger.LogInformation($"Результат обработчика: { result }");
        return result;
    });

app.Run();


record Fruit(string Name, int Stock);

class ValidationHelper
{
    // context предоставляет аргументы метода конечной точки и HttpContext.
    // next представляет метод фильтра, который будет вызываться следующим в конвейере фильтров конечной точки.
    // Если фильтр в конвейере последний, то управление передается обработчику конечной точки.
    internal static async ValueTask<object?> ValidateId(EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        // Получение аргумента id из context.
        string id = context.GetArgument<string>(0);
        if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
            // В случае неверного формата аргумента id замыкает конвейер фильтров формирую ответ
            // в формате Problem Details.
            return Results.ValidationProblem(new Dictionary<string, string[]>()
            {
                { "id", new string[] { "Неверный формат. Идентификатор должен начинаться с 'f'." } }
            });

        // Вызов делегата next передает управление следующему фильтру в конвейере.
        // Если фильтр в конвейере последний, то управление передается обработчику конечной точки.
        return await next(context);
    }
}