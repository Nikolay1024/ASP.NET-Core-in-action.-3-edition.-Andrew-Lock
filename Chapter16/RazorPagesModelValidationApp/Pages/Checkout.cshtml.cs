using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesModelValidationApp.BindingModels;

namespace RazorPagesModelValidationApp.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public UserBindingModel BindingModel { get; set; } = new();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            // Сделайте что-нибудь, например, примите оплату, сохраните в базе данных и т. д.

            return RedirectToPage("Success");
        }
    }
}
