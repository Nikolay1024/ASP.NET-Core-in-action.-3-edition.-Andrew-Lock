using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesFluentValidationApp.BindingModels;
using RazorPagesFluentValidationApp.FluentValidation;

namespace RazorPagesFluentValidationApp.Pages
{
    public partial class CurrencyConverterModel : PageModel
    {
        [BindProperty]
        public CurrencyConverterBindingModel BindingModel { get; set; } = null!;
        public string[] Currencies { get; set; }
        public string? Results { get; set; }

        public CurrencyConverterModel(ICurrencyProvider provider) => Currencies = provider.GetCurrencies();

        public void OnGet()
        {
            BindingModel = new CurrencyConverterBindingModel()
            {
                CurrencyFrom = "CAD",
                CurrencyTo = "USD",
                Quantity = 50.00m,
            };
        }

        public void OnPost()
        {
            if (ModelState.IsValid)
                Results = $"Конвертация {BindingModel.CurrencyFrom} в {BindingModel.CurrencyTo}.";
            else
                Results = "Пожалуйста, исправьте ошибки.";
        }
    }
}
