using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesRouteTemplatesApp.Services;

namespace RazorPagesRouteTemplatesApp.Pages.ProductDetails;

public class IndexModel : PageModel
{
    readonly ProductService _productService;

    public IndexModel(ProductService productService) => _productService = productService;

    public Product? Selected { get; set; }

    public IActionResult OnGet(string name)
    {
        Selected = _productService.GetProduct(name);
        if (Selected is null)
            return NotFound();

        return Page();
    }
}
