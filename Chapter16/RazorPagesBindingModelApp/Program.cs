using RazorPagesBindingModelApp.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<DevTasksService>();

WebApplication app = builder.Build();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
