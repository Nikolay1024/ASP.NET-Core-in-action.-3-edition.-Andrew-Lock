using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using MinimalApiAuthenticationApp.Authorization;
using MinimalApiAuthenticationApp.Authorization.Filters;
using MinimalApiAuthenticationApp.Authorization.Handlers;
using MinimalApiAuthenticationApp.Authorization.Requirements;
using MinimalApiAuthenticationApp.Commands;
using MinimalApiAuthenticationApp.Data;
using MinimalApiAuthenticationApp.Services;
using MinimalApiAuthenticationApp.ViewModels;
using System.Security.Claims;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Регистрация сервисов аутентификации и авторизации.
// Метод AddJwtBearer() настраивает аутентификацию на основе JWT-токена на предъявителя.
builder.Services.AddAuthentication().AddJwtBearer();

//builder.Services.AddAuthorization();
// Настройка политик авторизации.
builder.Services.AddAuthorizationBuilder()
    .AddPolicy(AuthorizationPolicyNames.CanCreateRecipe,
        policyBuilder => policyBuilder.RequireClaim("iss", "dotnet-user-jwts"))
    .AddPolicy(AuthorizationPolicyNames.CanManageRecipe,
        policyBuilder => policyBuilder.AddRequirements(new IsRecipeOwnerRequirement()));

// Регистрация обработчика (handler) авторизации в контейнере внедрения зависимостей.
// Для этого приложения у обработчика нет зависимостей, внедряемых в конструктор, поэтому он регистрируется
// в контейнере как Singleton. Если у обработчиков есть зависимости с жизненным циклом Scoped или Transient
// то можно зарегистрировать их как Scoped. 
builder.Services.AddSingleton<IAuthorizationHandler, IsRecipeOwnerHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "Рецепты.", Version = "v1" });

    // Схема безопасности, используемая API.
    var securityScheme = new OpenApiSecurityScheme()
    {
        // Обязательно. Тип схемы безопасности (ApiKey/Http/OAuth2/OpenIdConnect).
        Type = SecuritySchemeType.ApiKey,
        // Обязательно. Где будет предоставлен токен (Path/Query/Header/Cookie).
        In = ParameterLocation.Header,
        // Обязательно. Имя используемого HTTP-заголовка.
        Name = HeaderNames.Authorization,
        // Scheme = "Bearer",               // Необязательно, Swashbuckle не использует в этом случае.
        // BearerFormat = "JWT",            // Необязательно, Swashbuckle не использует в этом случае.

        // Дружественное описание схемы, используемой в пользовательском интерфейсе.
        // JWT Authorization header using the Bearer scheme.
        Description = "HTTP-заголовок авторизации с JWT-токеном на предъявителя.",
        Reference = new OpenApiReference()
        {
            // Уникальный идентификатор схемы. Здесь используется имя схемы JWT по умолчанию.
            Id = JwtBearerDefaults.AuthenticationScheme,
            // Обязательно. Тип объекта OpenID.
            Type = ReferenceType.SecurityScheme,
        }
    };

    // Добавляет схему безопасности в документ OpenAPI для генерации Swagger UI.
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);

    // Добавляет требование безопасности глобально ко всему API (ко всем конечным точкам).
    options.AddSecurityRequirement(new OpenApiSecurityRequirement() { { securityScheme, Array.Empty<string>() } });
});

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
app.UseSwaggerUI();

app.UseRouting();

// Компоненты Authentication и Authorization размещаются явно после компонентов Swagger и SwaggerUI,
// чтобы интерфейс Swagger мог открыться без аутентификации.
app.UseAuthentication();
app.UseAuthorization();

RouteGroupBuilder routes = app.MapGroup("recipes").WithParameterValidation()
    .WithOpenApi().WithTags("Recipes")
    // Добавляет политику авторизации в группу конечных точек минимального API.
    .RequireAuthorization();

#region Map endpoints
routes.MapGet("/", async (RecipeService service) => await service.GetRecipes())
    .WithSummary("Список рецептов.")
    .ProducesProblem(StatusCodes.Status401Unauthorized);

routes.MapGet("/{id}", async (int id, RecipeService service) =>
{
    RecipeViewModel? recipe = await service.GetRecipeDetail(id);
    return recipe is null ? Results.Problem(statusCode: StatusCodes.Status404NotFound) : Results.Ok(recipe);
})
    .WithName("GetRecipeDetail").WithSummary("Получить рецепт.")
    .Produces<RecipeViewModel>()
    .ProducesProblem(StatusCodes.Status401Unauthorized).ProducesProblem(StatusCodes.Status404NotFound);

routes.MapPost("/", async (RecipeCreateCommand input, RecipeService service, ClaimsPrincipal user) =>
{
    int id = await service.CreateRecipe(input, user);
    return Results.CreatedAtRoute("GetRecipeDetail", new { id });
})
    .WithSummary("Создать рецепт.")
    .Produces(StatusCodes.Status201Created)
    .ProducesProblem(StatusCodes.Status401Unauthorized)
    // Добавляем политику авторизации.
    .RequireAuthorization(AuthorizationPolicyNames.CanCreateRecipe);

routes.MapPut("/", async (RecipeUpdateCommand input, RecipeService service) =>
{
    if (await service.IsAvailableForManage(input.Id))
    {
        await service.UpdateRecipe(input);
        return Results.NoContent();
    }
    return Results.Problem(statusCode: StatusCodes.Status404NotFound);
})
    .WithSummary("Обновить рецепт.")
    .Produces(StatusCodes.Status204NoContent)
    .ProducesProblem(StatusCodes.Status401Unauthorized).ProducesProblem(StatusCodes.Status403Forbidden)
    .ProducesProblem(StatusCodes.Status404NotFound)
    // Добавляем политику авторизации с использованием фильтра.
    .AddEndpointFilter<CanManageRecipeFilter>();

routes.MapDelete("/{id}", async (int id, RecipeService service) =>
{
    if (await service.IsAvailableForManage(id))
    {
        await service.DeleteRecipe(id);
        return Results.NoContent();
    }
    return Results.Problem(statusCode: StatusCodes.Status404NotFound);
})
    .WithSummary("Удалить рецепт.")
    .Produces(StatusCodes.Status204NoContent)
    .ProducesProblem(StatusCodes.Status401Unauthorized).ProducesProblem(StatusCodes.Status403Forbidden)
    .ProducesProblem(StatusCodes.Status404NotFound)
    // Добавляем политику авторизации с использованием фильтра.
    .AddEndpointFilter<CanManageRecipeFilter>();
#endregion

app.Run();
