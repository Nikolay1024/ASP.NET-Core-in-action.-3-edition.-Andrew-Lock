using IdentityApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// ASP.NET Core Identity использует EF Core, поэтому включает стандартную конфигурацию EF Core.
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Добавляет необязательные сервисы, чтобы улучшить страницу ошибок для разработчика.
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Добавляет систему Identity, включая пользовательский интерфейс по умолчанию,
// и настраивает тип пользователя как IdentityUser.
// RequireConfirmedAccount требует от пользователя подтверждения его учетной записи (обычно по электронной почте),
// перед тем как они выполнят вход.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    // Настраивает Identity для хранения ее данных в  EF Core.
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// WebApplication неявно добавляет компонент аутентификации AuthenticationMiddleware перед компонентом авторизации
// AuthorizationMiddleware в конвейер обработки HTTP-запроса.
// При желании можно изменить местоположение компонента аутентификации, вызвав метод UseAuthentication() в
// соответствующем месте. Единственное требование состоит в том, что компонент аутентификации должен быть размещен перед
// любым компонентом, требующим аутентификации пользователя, например перед компонентом авторизации.
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
