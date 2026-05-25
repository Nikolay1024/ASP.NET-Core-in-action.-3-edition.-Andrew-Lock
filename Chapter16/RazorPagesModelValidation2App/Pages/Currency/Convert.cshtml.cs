using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesModelValidation2App.BindingModels;

namespace RazorPagesModelValidation2App.Pages.Currency
{
    public class ConvertModel : PageModel
    {
        [BindProperty]
        public CurrencyConvertBindingModel BindingModel { get; set; } = new();

        public void OnGet() { }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            return RedirectToPage("Success");
        }
    }
}
