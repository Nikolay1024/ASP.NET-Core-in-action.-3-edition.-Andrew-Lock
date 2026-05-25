using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesBindingModel4App.Pages
{
    public class ConvertModel : PageModel
    {
        public string Values { get; set; } = string.Empty;

        public void OnGet(string currencyIn, string currencyOut, int qty)
        {
            Values = $"CurrencyIn: {currencyIn}.\nCurrencyOut: {currencyOut}.\nQty: {qty}.";
        }

        public void OnPost(string currencyIn, string currencyOut, int qty)
        {
            Values = $"CurrencyIn: {currencyIn}.\nCurrencyOut: {currencyOut}.\nQty: {qty}.";
        }
    }
}
