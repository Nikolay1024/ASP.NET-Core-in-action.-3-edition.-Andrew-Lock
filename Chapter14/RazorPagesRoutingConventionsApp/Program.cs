using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Метод расширения AddRazorPagesOptions() используется для настройки соглашений Razor Pages.
builder.Services.AddRazorPages().AddRazorPagesOptions((RazorPagesOptions options) =>
{
    // Добавление префикса /page ко всем страницам приложения.
    //options.Conventions.Add(new PrefixingPageRouteModelConvention());
    // Добавление префикса /page к странице /Privacy.
    //options.Conventions.AddPageRouteModelConvention("/Privacy", new PrefixingPageRouteModelConvention().Apply);

    // Регистрирует преобразователь шаблонов маршрута, применяемый ко всем страницам приложения.
    options.Conventions.Add(new PageRouteTransformerConvention(new KebabCaseParameterTransformer()));
    // Метод AddPageRoute() добавляет дополнительный шаблон маршрута для ProductDetails/Search.
    options.Conventions.AddPageRoute("/ProductDetails/Search", "search-products");
});

// Изменяет соглашения, используемые для создания URL-адресов. По умолчанию эти свойства имеют значение false.
builder.Services.Configure<RouteOptions>(options =>
{
    options.AppendTrailingSlash = true;
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


// Соглашение реализует IPageRouteModelConvention.
class PrefixingPageRouteModelConvention : IPageRouteModelConvention
{
    // ASP.NET Core вызывает Apply() при запуске приложения.
    public void Apply(PageRouteModel pageRoute)
    {
        List<SelectorModel> selectors = pageRoute.Selectors.Select(s => new SelectorModel()
        {
            AttributeRouteModel = new AttributeRouteModel()
            {
                Template = AttributeRouteModel.CombineTemplates("page", s.AttributeRouteModel!.Template),
            }
        }).ToList();

        foreach (SelectorModel selector in selectors)
            pageRoute.Selectors.Add(selector);
    }
}

partial class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        if (value is null)
            return null;

        return MyRegex().Replace(value.ToString()!, "$1-$2").ToLower();
    }

    // Регулярное выражение помогает заменить стиль шаблонов маршрутов с PascalCase на kebab-case.
    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}
