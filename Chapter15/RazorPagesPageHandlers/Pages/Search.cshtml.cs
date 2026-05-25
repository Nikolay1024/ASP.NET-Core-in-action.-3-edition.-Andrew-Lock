using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesPageHandlers.Services;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesPageHandlers.Pages
{
    public class BindingModel
    {
        [Required]
        public string SearchTerm { get; set; } = string.Empty;
    }

    public class SearchModel : PageModel
    {
        readonly SearchService _searchService;

        public SearchModel(SearchService searchService) => _searchService = searchService;

        // Свойства модели страницы, отмеченные атрибутом [BindProperty], будут привязаны к модели привязки.
        // По умолчанию этот атрибут ничего не делает для GET-запросов. Чтобы переопределить такое поведение
        // можно использовать свойство SupportsGet: [BindProperty(SupportsGet = true)].
        [BindProperty]
        public BindingModel Input { get; set; } = new BindingModel();
        public List<Product> Results { get; set; } = new List<Product>();

        // При выполнении страницы Razor вызывается обработчик страницы на основе HTTP-метода запроса (GET, POST, PUT,
        // DELETE) и параметра маршрута. Если соответствующий обработчик страницы не найден, вместо него используется
        // "неявный обработчик", просто отображающий содержимое страницы Razor.

        public void OnGet() { }

        // Параметры передаваемые в обработчик страницы привязываются к модели привязки и используют значения из запроса.
        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                Results = _searchService.SearchProducts(Input.SearchTerm);
                // Здесь тип возвращаемого значения PageResult, реализующий IActionResult.
                return Page();
            }

            // Здесь тип возвращаемого значения RedirectToPageResult, реализующий IActionResult.
            return RedirectToPage();
        }

        // Значение маршрута {Handler?} обычно берется из значения строки запроса в URL-адресе запроса, например
        // /Search?Handler=CustomSearch. Но можно включить параметр маршрута {Handler?} в шаблон маршрута страницы Razor.
        public void OnPostCustomSearch() { }
    }
}
