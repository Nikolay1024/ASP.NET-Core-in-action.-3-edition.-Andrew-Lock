using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesModelValidation3App.BindingModels;
using RazorPagesModelValidation3App.Models;
using RazorPagesModelValidation3App.Services;

namespace RazorPagesModelValidation3App.Pages
{
    public class EditProductModel : PageModel
    {
        readonly ProductService _productService;

        public EditProductModel(ProductService productService) => _productService = productService;

        [BindProperty]
        public EditProductBindingModel BindingModel { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            ProductDetails? product = _productService.GetProduct(id);
            if (product is null)
                return NotFound();

            BindingModel = new EditProductBindingModel()
            {
                Name = product.Name,
                Price = product.Price,
            };

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
                return Page();

            ProductDetails? product = _productService.GetProduct(id);
            if (product is null)
                return NotFound();

            _productService.UpdateProduct(id, BindingModel.Name, BindingModel.Price);

            return RedirectToPage("Index");
        }
    }
}
