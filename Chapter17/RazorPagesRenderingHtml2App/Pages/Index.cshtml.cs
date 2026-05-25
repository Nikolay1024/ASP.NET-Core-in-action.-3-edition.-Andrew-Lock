using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesRenderingHtml2App.Pages
{
    // Переменные, определенные в словаре ViewData[], служат для передачи данных
    // в макет (_Layout), секции (_Section) и частичные представления (_PartialView).
    // Переменные, определенные в словаре ViewData[], могут быть заданы в модели страницы или
    // в представлении Razor.
    public class IndexModel : PageModel
    {
        [ViewData]
        public bool DataFromProperty { get; set; } = true;

        public void OnGet()
        {
            ViewData["DataFromModelPage"] = true;
        }
    }
}
