using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MinimalApiBackgroundService2App.BackgroundServices;
using MinimalApiBackgroundService2App.Data;
using MinimalApiBackgroundService2App.HttpClients;
using MinimalApiBackgroundService2App.Options;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.WriteIndented = true);
builder.Services.AddOptions<RemoteApiOptions>().BindConfiguration("RemoteApi");

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddHttpClient<RemoteApiHttpClient>(
    (IServiceProvider serviceProvider, HttpClient httpClient) =>
    {
        IOptions<RemoteApiOptions> options = serviceProvider.GetRequiredService<IOptions<RemoteApiOptions>>();
        httpClient.BaseAddress = new Uri(options.Value.Url);
    });
// –егистраци€ фонового сервиса RemoteApiBackgroundService с жизненным циклом Singleton.
builder.Services.AddHostedService<RemoteApiBackgroundService>();

WebApplication app = builder.Build();

app.MapGet("/db", (AppDbContext dbContext) => dbContext.Albums);
app.MapGet("/http-client", async (RemoteApiHttpClient httpClient) => await httpClient.GetAlbumsAsync());

app.Run();
