using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesRenderingHtml3App.Services;

namespace RazorPagesRenderingHtml3App.Pages
{
    public class IndexModel : PageModel
    {
        readonly HouseTasksService _houseTasksService;
        public List<HouseTask> HouseTasks { get; set; } = new List<HouseTask>();

        public IndexModel(HouseTasksService houseTasksService) => _houseTasksService = houseTasksService;

        public void OnGet() => HouseTasks = _houseTasksService.HouseTasks;
    }
}
