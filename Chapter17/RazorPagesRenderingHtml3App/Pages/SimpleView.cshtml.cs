using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesRenderingHtml3App.Services;

namespace RazorPagesRenderingHtml3App.Pages
{
    public class SimpleViewModel : PageModel
    {
        readonly HouseTasksService _houseTasksService;
        public HouseTask? HouseTask { get; set; }

        public SimpleViewModel(HouseTasksService houseTasksService) => _houseTasksService = houseTasksService;

        public IActionResult OnGet(int id)
        {
            HouseTask = _houseTasksService.HouseTasks.FirstOrDefault(t => t.Id == id);
            if (HouseTask == null)
                return RedirectToPage("Index");

            return Page();
        }
    }
}
