WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

WebApplication app = builder.Build();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
