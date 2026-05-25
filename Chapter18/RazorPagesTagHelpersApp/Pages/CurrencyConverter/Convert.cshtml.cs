using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesTagHelpersApp.BindingModels;

namespace RazorPagesTagHelpersApp.Pages.CurrencyConverter
{
    public class ConvertModel : PageModel
    {
        [BindProperty]
        public ConvertBindingModel BindingModel { get; set; } = new();
        public List<SelectListItem> CurrencyCodes { get; } = new()
        {
            new("GBP", "GBP"),
            new("USD", "USD"),
            new("CAD", "CAD"),
            new("EUR", "EUR"),
        };

        public void OnGet() { }
        public IActionResult OnPost()
        {
            // Тег-хелпер сводки валидации особенно полезен, если у вас есть ошибки, связанные с вашей страницей,
            // которые не относятся только к одному свойству. Такие ошибки можно добавить к состоянию модели,
            // используя пустой ключ.
            // В этом примере валидация свойств прошла успешно, но мы предоставляем дополнительную проверку на
            // уровне модели, чтобы убедиться, что мы не пытаемся конвертировать валюту в саму себя.
            if (BindingModel.CurrencyFrom == BindingModel.CurrencyTo)
                ModelState.AddModelError(string.Empty, "Невозможно конвертировать валюту в себя.");

            // Повторно отправленная форма отобразит ошибки валидции уровня свойств и уровня модели в
            // соответствующих тегах <span/> и <div/> в случае их наличия.
            if (!ModelState.IsValid)
                return Page();

            // TODO: Сохраните действительные значения в базе данных, выполните преобразование.

            return RedirectToPage("Success");
        }
    }
}
