using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorPagesSelectTagHelperApp.BindingModels;

namespace RazorPagesSelectTagHelperApp.Pages
{
    public class IndexModel : PageModel
    {
        // Создает один экземпляр каждой группы для передачи в ItemsWithGroups для создания списка с группами.
        static readonly SelectListGroup _dynamicGroup = new() { Name = "Dynamic" };
        static readonly SelectListGroup _staticGroup = new() { Name = "Static" };

        // Списки элементов для отображения в полях выпадающих списков.
        public List<SelectListItem> Items { get; set; } = new()
        {
            new("JavaScript", "js"),
            new("Python", "python"),
            new("C++", "cpp"),
            new("C#", "csharp"),
        };

        // Устанавливает соответствующую группу для каждого SelectListItem.
        // Если у SelectListItem нет группы, он не будет добавлен в тег <optgroup/>.
        public List<SelectListItem> ItemsWithGroups { get; set; } = new()
        {
            new("JavaScript", "js") { Group = _dynamicGroup },
            new("Python", "python") { Group = _dynamicGroup },
            new("C++", "cpp") { Group = _staticGroup },
            new("C#", "csharp"),
        };

        public List<SelectListItem> ItemsWithNotSelected { get; set; } = new()
        {
            new("--Не выбрано--", null),
            new("JavaScript", "js"),
            new("Python", "python"),
            new("C++", "cpp"),
            new("C#", "csharp"),
        };

        [BindProperty]
        public BindingModel BindingModel { get; set; } = new();

        public void OnGet() { }
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                Page();

            return RedirectToPage("Success");
        }
    }
}
