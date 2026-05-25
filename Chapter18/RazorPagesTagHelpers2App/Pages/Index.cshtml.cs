using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesTagHelpers2App.BindingModels;

namespace RazorPagesTagHelpers2App.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public BindingModel BindingModel { get; set; } = new();

        public void OnGet() { }
        public void OnPost()
        {
            ModelState.AddModelError(string.Empty,
                "Если кроме этой ошибки валидации других ошибок нет, то валидация прошла успешно.");
            if (!ModelState.IsValid)
                Page();
        }
    }
}
