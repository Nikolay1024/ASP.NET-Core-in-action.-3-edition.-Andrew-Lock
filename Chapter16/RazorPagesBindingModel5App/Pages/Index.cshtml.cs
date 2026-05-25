using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesBindingModel5App.Services;

namespace RazorPagesBindingModel5App.Pages
{
    public class IndexModel : PageModel
    {
        private readonly CurrencyService _currencyService;

        public IndexModel(CurrencyService currencyService) => _currencyService = currencyService;

        public List<SelectListItem> Currencies { get; set; } = new();

        public void OnGet()
        {
            Currencies = _currencyService.Currencies.Select(c => new SelectListItem() { Text = c.Key }).ToList();
        }
    }
}
