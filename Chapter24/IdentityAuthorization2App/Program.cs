using IdentityAuthorization2App.Authorization.Handlers;
using IdentityAuthorization2App.Authorization.Requirements;
using IdentityAuthorization2App.Data;
using IdentityAuthorization2App.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddRazorPages();

builder.Services.AddScoped<RecipeService>();

// Метод AddAuthorizationBuilder() добавляет сервисы авторизации.
builder.Services.AddAuthorizationBuilder()
    // Метод AddPolicy() добавляет политику авторизации с именем CanManageRecipe.
    .AddPolicy("CanManageRecipe", policyBuilder => policyBuilder
        // Добавляет требование (requirement) политики как экземпляр реализации IAuthorizationRequirement.
        .AddRequirements(new IsRecipeOwnerRequirement()));

// Регистрация обработчика (handler) авторизации в контейнере внедрения зависимостей.
// Для этого приложения у обработчика есть зависимость UserManager<AppUser>, поэтому он регистрируется
// в контейнере как Scoped. 
builder.Services.AddScoped<IAuthorizationHandler, IsRecipeOwnerHandler>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
