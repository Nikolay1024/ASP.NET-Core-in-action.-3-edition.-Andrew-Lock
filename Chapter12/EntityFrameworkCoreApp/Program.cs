using EntityFrameworkCoreApp;
using EntityFrameworkCoreApp.Commands;
using EntityFrameworkCoreApp.Data;
using EntityFrameworkCoreApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => x.SwaggerDoc("v1", new OpenApiInfo() { Title = "Рецепты.", Version = "v1" }));

// Строка подключения берется из конфигурации, из секции ConnectionStrings.
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Регистрируем DbContext приложения, передавая его в качестве обобщенного параметра методу AddDbContext<T>.
// Указываем провайдера базы данных в параметрах настройки с помощью метода UseSqlServer().
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString!));

builder.Services.AddScoped<RecipeService>();
builder.Services.AddProblemDetails();

WebApplication app = builder.Build();

app.UseExceptionHandler();

app.UseSwagger();
app.UseSwaggerUI((SwaggerUIOptions options) =>
{
    // Устанавливает путь конечной точки для Swagger UI / вместо /swagger.
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

RouteGroupBuilder routes = app.MapGroup("recipes").WithParameterValidation()
    .WithOpenApi().WithTags("Recipes");

routes.MapGet("/", async (RecipeService service) => await service.GetRecipes())
    .WithSummary("Список рецептов.");

routes.MapGet("/{id}", async (int id, RecipeService service) =>
{
    RecipeViewModel? recipe = await service.GetRecipeDetail(id);
    return recipe is null ? Results.Problem(statusCode: 404) : Results.Ok(recipe);
})
    .WithName("GetRecipe").WithSummary("Получить рецепт.").ProducesProblem(404).Produces<RecipeViewModel>();

routes.MapPost("/", async (RecipeCreateCommand input, RecipeService service) =>
{
    int id = await service.CreateRecipe(input);
    return Results.CreatedAtRoute("GetRecipe", new { id });
})
    .WithSummary("Создать рецепт.").Produces(201);

routes.MapPut("/", async (RecipeUpdateCommand input, RecipeService service) =>
{
    if (await service.IsAvailableForUpdate(input.Id))
    {
        await service.UpdateRecipe(input);
        return Results.NoContent();
    }
    return Results.Problem(statusCode: 404);
})
    .WithSummary("Обновить рецепт.").ProducesProblem(404).Produces(204);

routes.MapDelete("/{id}", async (int id, RecipeService service) =>
{
    await service.DeleteRecipe(id);
    return Results.NoContent();
})
    .WithSummary("Удалить рецепт.").Produces(204);

app.Run();
