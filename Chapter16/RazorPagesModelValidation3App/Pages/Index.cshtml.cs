using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesModelValidation3App.Models;
using RazorPagesModelValidation3App.Services;

namespace RazorPagesModelValidation3App.Pages
{
    public class IndexModel : PageModel
    {
        readonly ProductService _productService;

        public IndexModel(ProductService productService) => _productService = productService;

        public Dictionary<int, ProductDetails> Products { get; private set; } = new();

        public void OnGet() => Products = _productService.Products;
    }
}
