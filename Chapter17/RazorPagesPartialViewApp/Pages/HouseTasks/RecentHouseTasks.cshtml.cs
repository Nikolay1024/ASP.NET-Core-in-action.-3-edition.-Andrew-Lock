using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesPartialViewApp.Services;

namespace RazorPagesPartialViewApp.Pages.HouseTasks
{
    public class RecentHouseTasksModel : PageModel
    {
        readonly HouseTasksService _houseTasksService;
        public int RecentHouseTasksToDisplay { get; } = 3;
        public List<HouseTask> RecentHouseTasks { get; set; } = new();

        public RecentHouseTasksModel(HouseTasksService houseTasksService) => _houseTasksService = houseTasksService;

        public void OnGet() =>
            RecentHouseTasks = _houseTasksService.HouseTasks.Take(RecentHouseTasksToDisplay).ToList();
    }
}
