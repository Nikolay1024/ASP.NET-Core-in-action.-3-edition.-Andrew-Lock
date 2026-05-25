using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesBindingModel5App.Services;

namespace RazorPagesBindingModel5App.Pages
{
    public class ShowRatesModel : PageModel
    {
        readonly CurrencyService _currencyService;
        
        public ShowRatesModel(CurrencyService currencyService) => _currencyService = currencyService;

        public Dictionary<string, decimal> SelectedCurrencies { get; set; } = new();

        public void OnPost(List<string> currencies)
        {
            SelectedCurrencies = _currencyService.Currencies.Where(c => currencies.Contains(c.Key))
                .ToDictionary(c => c.Key, c => c.Value);
        }
    }
}
