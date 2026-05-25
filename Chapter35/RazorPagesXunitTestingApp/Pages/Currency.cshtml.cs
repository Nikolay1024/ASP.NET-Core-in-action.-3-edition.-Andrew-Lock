using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using RazorPagesXunitTestingApp.BindingModels;
using RazorPagesXunitTestingApp.Options;
using RazorPagesXunitTestingApp.Services;
using System.ComponentModel;

namespace RazorPagesXunitTestingApp.Pages
{
    public class CurrencyModel : PageModel
    {
        private readonly CurrencyOptions _currencyOptions;
        private readonly ICurrencyService _currencyService;

        [BindProperty]
        public CurrencyBindingModel BindingModel { get; set; } = null!;

        [DisplayName("Результат преобразования валюты GBP -> USD")]
        public decimal Result { get; set; }

        public CurrencyModel(IOptions<CurrencyOptions> currencyOptions, ICurrencyService currencyService)
        {
            _currencyOptions = currencyOptions.Value;
            _currencyService = currencyService;
        }

        public void OnGet()
        {
            BindingModel = new CurrencyBindingModel()
            {
                Quantity = _currencyOptions.DefaultQuantity,
                ExchangeRate = _currencyOptions.DefaultExchangeRate,
                DecimalPlaces = _currencyOptions.DefaultDecimalPlaces,
            };
        }

        public PageResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            Result = _currencyService.ConvertToGbp(BindingModel.Quantity, BindingModel.ExchangeRate,
                BindingModel.DecimalPlaces);

            return Page();
        }
    }
}
