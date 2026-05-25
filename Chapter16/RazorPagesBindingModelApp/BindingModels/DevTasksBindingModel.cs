using System.ComponentModel.DataAnnotations;

namespace RazorPagesBindingModelApp.BindingModels
{
    public class DevTasksBindingModel
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string State { get; set; } = string.Empty;
    }
}
