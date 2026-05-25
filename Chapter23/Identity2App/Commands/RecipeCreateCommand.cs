using Identity2App.Data;

namespace Identity2App.Commands
{
    public class RecipeCreateCommand : RecipeCommandBase
    {
        public IList<IngredientCreateCommand> Ingredients { get; set; } = new List<IngredientCreateCommand>();

        // Создаем рецепт путем отображения из объекта команды в сущность Recipe.
        public Recipe ToRecipe()
        {
            return new Recipe()
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                Ingredients = Ingredients.Select(i => i.ToIngredient()).ToList(),
            };
        }
    }
}
