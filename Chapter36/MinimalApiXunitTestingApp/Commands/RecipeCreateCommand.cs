using MinimalApiXunitTestingApp.Data;

namespace MinimalApiXunitTestingApp.Commands
{
    public class RecipeCreateCommand : RecipeCommandBase
    {
        public IList<IngredientCreateCommand> Ingredients { get; set; } = new List<IngredientCreateCommand>();

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
