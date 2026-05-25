using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTagHelpersApp.BindingModels;

namespace RazorPagesTagHelpersApp.Pages.CurrencyConverter
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public CheckoutBindingModel BindingModel { get; set; } = new();

        public void OnGet() { }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("Success");
        }
    }
}
