using RazorPages2App.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<ToDoService>();

WebApplication app = builder.Build();

app.MapRazorPages();

app.Map("/", () => Results.Redirect("/category/simple"));

app.Run();
