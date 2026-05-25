using IdentityAuthorization2App.Data;

namespace IdentityAuthorization2App.Commands
{
    public class RecipeCreateCommand : RecipeCommandBase
    {
        public IList<IngredientCreateCommand> Ingredients { get; set; } = new List<IngredientCreateCommand>();

        // Создаем рецепт путем отображения из объекта команды в сущность Recipe.
        public Recipe ToRecipe(AppUser createdBy)
        {
            return new Recipe()
            {
                Name = Name,
                TimeToCook = new TimeSpan(TimeToCookHrs, TimeToCookMins, 0),
                Method = Method,
                IsVegetarian = IsVegetarian,
                IsVegan = IsVegan,
                LastModified = DateTimeOffset.UtcNow,
                CreatedBy = createdBy.Id,
                Ingredients = Ingredients.Select(i => i.ToIngredient()).ToList(),
            };
        }
    }
}
