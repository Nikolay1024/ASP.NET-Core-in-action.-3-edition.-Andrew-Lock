WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
// Настраивает RouteOptions, используемый для генерации ссылок.
builder.Services.Configure<RouteOptions>(options =>
{
    // По умолчанию для всех настроек задано значение false.
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});
WebApplication app = builder.Build();

app.MapGet("/HealthCheck", () => Results.Ok()).WithName("healthcheck");
app.MapGet("/{name}", (string name) => name).WithName("product");
app.MapGet("/test1", (LinkGenerator links) =>
    new string?[]
    {
        // Возвращает /healthcheck/.
        links.GetPathByName("healthcheck"),
        // Возвращает /big-widget/?q=test.
        links.GetPathByName("product", new { Name = "Big-Widget", Q = "Test" })
    });
var linkOptions = new LinkOptions()
{
    AppendTrailingSlash = false,
    LowercaseUrls = false,
    LowercaseQueryStrings = false,
};
app.MapGet("/test2", (LinkGenerator links) =>
    new string?[]
    {
        // Возвращает /HealthCheck.
        links.GetPathByName("healthcheck", options: linkOptions),
        // Возвращает /Big-Widget?Q=Test.
        links.GetPathByName("product", new { Name = "Big-Widget", Q = "Test" }, options: linkOptions)
    });

app.Run();
