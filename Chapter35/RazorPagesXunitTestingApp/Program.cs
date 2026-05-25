using RazorPagesXunitTestingApp.Options;
using RazorPagesXunitTestingApp.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSingleton<ICurrencyService, CurrencyService>();
builder.Services.AddOptions<CurrencyOptions>().BindConfiguration("CurrencyOptions");

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
