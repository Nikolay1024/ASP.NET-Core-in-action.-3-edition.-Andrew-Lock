using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesRouteTemplatesApp.Services;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesRouteTemplatesApp.Pages;

public class SearchModel : PageModel
{
    readonly LinkGenerator _linkGenerator;
    readonly ProductService _productService;

    public SearchModel(LinkGenerator linkGenerator, ProductService productService)
    {
        _linkGenerator = linkGenerator;
        _productService = productService;
    }

    [BindProperty, Required, Display(Name = "Поиск")]
    public string SearchTerm { get; set; } = string.Empty;
    public List<string?> Urls { get; set; } = new();
    public List<Product> Products { get; private set; } = new();

    public void OnGet()
    {
        // Примеры генерации ссылок.
        Urls = new List<string?>()
        {
            Url.Page("ProductDetails/Index", new { name = "big-widget" }),
            _linkGenerator.GetPathByPage("/ProductDetails/Index", values: new { name = "big-widget" }),
            _linkGenerator.GetPathByPage(HttpContext, "/ProductDetails/Index", values: new { name = "big-widget" }),
            _linkGenerator.GetUriByPage("/ProductDetails/Index", handler: null, values: new { name = "big-widget" },
            scheme: "https", host: new HostString("example.com")),
        };
    }

    public void OnPost()
    {
        if (ModelState.IsValid)
            Products = _productService.Search(SearchTerm, StringComparison.Ordinal);
    }
    public void OnPostIgnoreCase()
    {
        if (ModelState.IsValid)
            Products = _productService.Search(SearchTerm, StringComparison.OrdinalIgnoreCase);
    }
}