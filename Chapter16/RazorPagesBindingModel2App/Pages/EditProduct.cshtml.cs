using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesBindingModel2App.Models;

namespace RazorPagesBindingModel2App.Pages
{
    [IgnoreAntiforgeryToken]
    public class EditProductModel : PageModel
    {
        public ProductModel Product { get; set; } = new();

        public void OnGet() { }
        public void OnPostEditProduct(ProductModel product) => Product = product;
        public void OnPostEditTwoProducts(ProductModel product1, ProductModel product2) => Product = product1;
    }
}
