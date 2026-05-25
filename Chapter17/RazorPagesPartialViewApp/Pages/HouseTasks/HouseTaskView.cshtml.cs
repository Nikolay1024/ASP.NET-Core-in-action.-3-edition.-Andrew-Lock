using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesPartialViewApp.Services;

namespace RazorPagesPartialViewApp.Pages.HouseTasks
{
    public class HouseTaskViewModel : PageModel
    {
        readonly HouseTasksService _houseTasksService;
        public HouseTask? HouseTask { get; set; }

        public HouseTaskViewModel(HouseTasksService houseTasksService) => _houseTasksService = houseTasksService;

        public IActionResult OnGet(int id)
        {
            HouseTask = _houseTasksService.HouseTasks.FirstOrDefault(t => t.Id == id);
            if (HouseTask == null)
                return RedirectToPage("RecentHouseTasks");

            return Page();
        }
    }
}
