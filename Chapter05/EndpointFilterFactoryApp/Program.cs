using System.Collections.Concurrent;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();
ConcurrentDictionary<string, Fruit> fruits = new();

app.MapGet("/fruit/{id}", (string id) =>
        fruits.TryGetValue(id, out Fruit? fruit) ? TypedResults.Ok(fruit) : Results.Problem(statusCode: 404))
    // Фабрика фильтров может обрабатывать конечные точки с различными сигнатурами методов.
    .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

app.MapPost("/fruit/{id}", (Fruit fruit, string id) =>
        fruits.TryAdd(id, fruit) ? TypedResults.Created($"/fruit/{id}", fruit) :
            Results.ValidationProblem(new Dictionary<string, string[]>()
            {
                { "id", new string[] { "Фрукт с таким идентификатором уже существует." } }
            }))
   // Фабрика фильтров может обрабатывать конечные точки с различными сигнатурами методов.
   .AddEndpointFilterFactory(ValidationHelper.ValidateIdFactory);

app.Run();


record Fruit(string Name, int Stock);

class ValidationHelper
{
    // factoryContext предоставляет информацию о методе обработчике конечной точки.
    // next представляет метод фильтра, который будет вызываться следующим в конвейере фильтров конечной точки.
    // Если фильтр в конвейере последний, то управление передается обработчику конечной точки.
    internal static EndpointFilterDelegate ValidateIdFactory(EndpointFilterFactoryContext factoryContext,
        EndpointFilterDelegate next)
    {
        // Метод GetParameters() предоставляет информацию о параметрах вызываемого обработчика.
        ParameterInfo[] parameters = factoryContext.MethodInfo.GetParameters();
        int? idPosition = null;
        for (int i = 0; i < parameters.Length; i++)
            if (parameters[i].Name == "id" && parameters[i].ParameterType == typeof(string))
            {
                idPosition = i;
                break;
            }

        // Если параметр id не найден, то фильтр не добавляется, а возвращается оставшаяся часть конвейера.
        if (!idPosition.HasValue)
            return next;

        // Если параметр id найден, то возвращается функция фильтра (фильтр, выполняемый для конечной точки).
        return async (EndpointFilterInvocationContext invocationContext) =>
        {
            string id = invocationContext.GetArgument<string>(idPosition.Value);
            if (string.IsNullOrEmpty(id) || !id.StartsWith('f'))
                // В случае неверного формата аргумента id замыкает конвейер фильтров формирую ответ
                // в формате Problem Details.
                return Results.ValidationProblem(new Dictionary<string, string[]>()
                {
                    { "id", new string[] { "Неверный формат. Идентификатор должен начинаться с 'f'." } }
                });

            // Вызов делегата next передает управление следующему фильтру в конвейере.
            // Если фильтр в конвейере последний, то управление передается обработчику конечной точки.
            return await next(invocationContext);
        };
    }
}
