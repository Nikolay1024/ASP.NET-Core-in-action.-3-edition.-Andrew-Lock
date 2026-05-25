using RazorPagesRouteTemplatesApp.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<ProductService>();

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
