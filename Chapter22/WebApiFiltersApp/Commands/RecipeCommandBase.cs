using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApiFiltersApp.Commands
{
    public abstract class RecipeCommandBase
    {
        [Required, StringLength(100)]
        public required string Name { get; set; }
        [Range(0, 23), DisplayName("Времени готовить (ч)")]
        public int TimeToCookHrs { get; set; }
        [Range(0, 59), DisplayName("Времени готовить (мин)")]
        public int TimeToCookMins { get; set; }
        [Required]
        public required string Method { get; set; }
        [DefaultValue(false), DisplayName("Вегетарианец?")]
        public bool IsVegetarian { get; set; }
        [DefaultValue(false), DisplayName("Веган?")]
        public bool IsVegan { get; set; }
    }
}
