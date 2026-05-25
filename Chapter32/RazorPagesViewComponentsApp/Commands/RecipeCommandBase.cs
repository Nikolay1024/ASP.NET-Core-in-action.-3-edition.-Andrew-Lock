using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace RazorPagesViewComponentsApp.Commands
{
    public abstract class RecipeCommandBase
    {
        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;
        [Range(0, 23), DisplayName("Времени готовить (ч)")]
        public int TimeToCookHrs { get; set; }
        [Range(0, 59), DisplayName("Времени готовить (мин)")]
        public int TimeToCookMins { get; set; }
        [Required]
        public string Method { get; set; } = string.Empty;
        [DefaultValue(false), DisplayName("Вегетарианец?")]
        public bool IsVegetarian { get; set; }
        [DefaultValue(false), DisplayName("Веган?")]
        public bool IsVegan { get; set; }
    }
}
