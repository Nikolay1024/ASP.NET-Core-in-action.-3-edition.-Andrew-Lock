using IdentityAuthorizationApp.Authorization;
using IdentityAuthorizationApp.Authorization.Handlers;
using IdentityAuthorizationApp.Authorization.Requirements;
using IdentityAuthorizationApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// Метод AddAuthorizationBuilder() добавляет сервисы авторизации.
builder.Services.AddAuthorizationBuilder()
    // Метод AddPolicy() добавляет политику авторизации с именем CanEnterSecurity.
    // И задает требование (requirement) политики с помощью лямбда-функции, где укзывается утверждение (claim)
    // пользователя.
    .AddPolicy("CanEnterSecurity", policyBuilder => policyBuilder.RequireClaim(Claims.BoardingPassNumber))
    // Добавляет новую политику для бизнес-зала аэропорта, CanAccessLounge.
    .AddPolicy("CanAccessLounge", policyBuilder => policyBuilder
        // Добавляет требования (requirements) политики как экземпляры каждой реализации IAuthorizationRequirement.
        .AddRequirements(new MinimumAgeRequirement(18), new AllowedInLoungeRequirement()));

// Регистрация обработчиков (handlers) авторизации в контейнере внедрения зависимостей.
// Для этого приложения у обработчиков нет зависимостей, внедряемых в конструктор, поэтому они регистрируются
// в контейнере как Singleton. Если у обработчиков есть зависимости с жизненным циклом Scoped или Transient
// то можно зарегистрировать их как Scoped. 
builder.Services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, FrequentFlyerHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, IsAirlineEmployeeHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, BannedFromLoungeHandler>();

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
