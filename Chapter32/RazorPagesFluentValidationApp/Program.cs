using FluentValidation;
using FluentValidation.AspNetCore;
using RazorPagesFluentValidationApp.FluentValidation;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services
    // ќтключает проверку DataAnnotations дл€ прив€зки модели.
    .AddFluentValidationAutoValidation(options => options.DisableDataAnnotationsValidation = true)
    // ¬ключает интеграцию с клиентской валидацией при помощи data-val-* атрибутов.
    .AddFluentValidationClientsideAdapters()
    // ¬ключает автоматическую регистрацию валидаторов в контейнере внедрени€ зависимостей.
    .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// FluentValidation имеет полную поддержку локализации, но вы можете отключить ее, если она вам не нужна.
ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.Services.AddSingleton<ICurrencyProvider, CurrencyProvider>();

WebApplication app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
