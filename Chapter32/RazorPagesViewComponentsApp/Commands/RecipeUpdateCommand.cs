using RazorPagesViewComponentsApp.Data;

namespace RazorPagesViewComponentsApp.Commands
{
    public class RecipeUpdateCommand : RecipeCommandBase
    {
        public int Id { get; set; }

        // Устанавливает новые значения для сущности Recipe.
        public void UpdateRecipe(Recipe recipe)
        {
            recipe.Name = Name;
            recipe.TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0);
            recipe.Method = Method;
            recipe.IsVegetarian = IsVegetarian;
            recipe.IsVegan = IsVegan;
        }
    }
}
