WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// AddControllersWithViews() добавляет сервисы для контроллеров MVC с представлениями Razor.
builder.Services.AddControllersWithViews();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // Путь обработчика исключений отличается от пути Razor Pages по умолчанию /Error.
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Регистрирует методы действий в контроллерах MVC в качестве конечных точек,
// используя маршрутизацию на основе соглашений.
app.MapControllerRoute(
    name: "default",
    // Определяет шаблон маршрута на основе соглашений по умолчанию.
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
