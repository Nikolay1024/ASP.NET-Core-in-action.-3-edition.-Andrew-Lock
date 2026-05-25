using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesRouteTemplatesApp.Services;

namespace RazorPagesRouteTemplatesApp.Pages;

public class ProductsModel : PageModel
{
    readonly ProductService _productService;

    public ProductsModel(ProductService productService) => _productService = productService;

    [BindProperty(SupportsGet = true)]
    public string Name { get; set; } = string.Empty;
    public Product? Selected { get; set; }

    public IActionResult OnGet()
    {
        Selected = _productService.GetProduct(Name);
        if (Selected is null)
            return NotFound();

        return Page();
    }
}