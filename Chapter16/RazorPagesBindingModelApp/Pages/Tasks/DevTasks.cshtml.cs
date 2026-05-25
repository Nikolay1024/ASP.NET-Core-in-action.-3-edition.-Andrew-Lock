using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesBindingModelApp.BindingModels;
using RazorPagesBindingModelApp.Services;
using RazorPagesBindingModelApp.ViewModels;

namespace RazorPagesBindingModelApp.Pages.Tasks
{
    public class DevTasksModel : PageModel
    {
        readonly DevTasksService _devTasksService;

        public DevTasksModel(DevTasksService devTasksService) => _devTasksService = devTasksService;

        [BindProperty(SupportsGet = true)]
        public DevTasksBindingModel BindingModel { get; set; } = new();

        public DevTasksViewModel ViewModel { get; set; } = new();

        public IActionResult OnGet()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //TODO: Проверить параметры.

            List<DevTaskModel> devTasksModel = _devTasksService.GetDevTasks(BindingModel.Username, BindingModel.State);

            ViewModel = new DevTasksViewModel(BindingModel.Username, BindingModel.State)
            {
                DevTasks = devTasksModel.Select(t => new DevTaskViewModel(t.Id, t.Title)).ToList(),
            };

            return Page();
        }
    }
}
