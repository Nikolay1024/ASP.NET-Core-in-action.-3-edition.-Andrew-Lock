using RazorPagesRenderingHtml3App.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<HouseTasksService>();

WebApplication app = builder.Build();

app.UseStaticFiles();

app.MapRazorPages();

app.Run();
