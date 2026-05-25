using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages2App.Services;

namespace RazorPages2App.Pages
{
    // Модель Razor.
    public class CategoryModel : PageModel
    {
        readonly ToDoService _service;
        // "Модель представления". Свойства доступные в представлении Razor, которые можно использовать для генерации
        // ответа в виде HTML.
        public List<ToDoListModel> Items { get; set; } = new List<ToDoListModel>();

        public CategoryModel(ToDoService service) => _service = service;

        // "Модель привязки". Параметр category строится из деталей, предоставленных в запросе (пути запроса, строки
        // запроса, тела запроса, заголовков запроса).
        // Модель привязки для обработчика страницы в Razor Pages такая же как и для конечных точек в Minimal API.
        public ActionResult OnGet(string category)
        {
            Items = _service.GetItemsForCategory(category);
            // Обработчик возвращает Page() в конце метода, чтобы указать, что должно быть отрисовано связанное
            // представление Razor. В этом случае оператор return фактически является необязательным. По соглашению,
            // если обработчик страницы возвращает void, представление Razor будет отображаться, как если бы вы вызвали
            // return Page() в конце метода.
            return Page();
        }
    }
}
