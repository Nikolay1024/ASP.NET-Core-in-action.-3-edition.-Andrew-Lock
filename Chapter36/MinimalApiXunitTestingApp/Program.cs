using Microsoft.EntityFrameworkCore;
using MinimalApiXunitTestingApp.BindingModels;
using MinimalApiXunitTestingApp.Commands;
using MinimalApiXunitTestingApp.Data;
using MinimalApiXunitTestingApp.Middlewares;
using MinimalApiXunitTestingApp.Services;
using MinimalApiXunitTestingApp.ViewModels;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails();

string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString!));

builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<RecipeService>();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseStatusCodePages();

app.UseMiddleware<StatusMiddleware>();

RouteGroupBuilder routes = app.MapGroup("").WithTags("Currency").WithParameterValidation();

#region Конечная точка Currency.
routes.MapPost("currency", (CurrencyBindingModel bindingModel, ICurrencyService currencyService) =>
{
    decimal content = currencyService.ConvertToGbp(bindingModel.Quantity, bindingModel.ExchangeRate,
        bindingModel.DecimalPlaces);
    return Results.Ok(content);
});
#endregion

#region Конечные точки Recipes.
routes = routes.MapGroup("recipes").WithTags("Recipes");

routes.MapGet("", async (RecipeService recipeService) => await recipeService.GetRecipes());

routes.MapGet("{id}", async (int id, RecipeService recipeService) =>
{
    RecipeViewModel? recipe = await recipeService.GetRecipeDetail(id);
    return recipe is null ? Results.Problem(statusCode: 404) : Results.Ok(recipe);
}).WithName("GetRecipe");

routes.MapPost("", async (RecipeCreateCommand input, RecipeService recipeService) =>
{
    int id = await recipeService.CreateRecipe(input);
    return Results.CreatedAtRoute("GetRecipe", new { id });
});

routes.MapPut("/", async (RecipeUpdateCommand input, RecipeService recipeService) =>
{
    if (await recipeService.IsAvailableForUpdate(input.Id))
    {
        await recipeService.UpdateRecipe(input);
        return Results.NoContent();
    }
    return Results.Problem(statusCode: 404);
});

routes.MapDelete("/{id}", async (int id, RecipeService recipeService) =>
{
    await recipeService.DeleteRecipe(id);
    return Results.NoContent();
});
#endregion

app.Run();

// Класс WebApplicationFactory<Program> должен ссылаться на public класс в вашем приложении. Обычно используются
// классы Program или Startup. Если вы пользуетесь операторами верхнего уровня (top-level statements, в .NET 7
// используются по умолчанию), то автоматически генерируемый класс по умолчанию internal. Чтобы сделать его
// public и тем самым предоставить его для тестового проекта, необходимо добавить определение partial класса в
// ваше приложение.
public partial class Program { }
