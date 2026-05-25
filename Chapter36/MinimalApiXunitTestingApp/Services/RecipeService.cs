using Microsoft.EntityFrameworkCore;
using MinimalApiXunitTestingApp.Commands;
using MinimalApiXunitTestingApp.Data;
using MinimalApiXunitTestingApp.ViewModels;

namespace MinimalApiXunitTestingApp.Services
{
    public class RecipeService
    {
        private readonly AppDbContext _dbContext;

        public RecipeService(AppDbContext dbContext) => _dbContext = dbContext;

        // Получение сводки рецептов из базы данных.
        public async Task<List<RecipeSummaryViewModel>> GetRecipes()
        {
            return await _dbContext.Recipes.Where(r => !r.IsDeleted)
                .Select(r => new RecipeSummaryViewModel()
                {
                    Id = r.RecipeId,
                    Name = r.Name,
                    TimeToCook = r.TimeToCook.ToString(@"hh\:mm"),
                    NumberOfIngredients = r.Ingredients.Count,
                })
                .ToListAsync();
        }

        // Получение рецепта по идентификатору из базы данных.
        public async Task<RecipeViewModel?> GetRecipeDetail(int id)
        {
            return await _dbContext.Recipes.Where(r => r.RecipeId == id && !r.IsDeleted)
                .Select(r => new RecipeViewModel()
                {
                    Id = r.RecipeId,
                    Name = r.Name,
                    Method = r.Method,
                    Ingredients = r.Ingredients.Select(i => new IngredientViewModel()
                    {
                        Name = i.Name,
                        Quantity = i.Quantity,
                        Unit = i.Unit,
                    }),
                })
                .SingleOrDefaultAsync();
        }

        // Создание рецепта в базе данных.
        public async Task<int> CreateRecipe(RecipeCreateCommand cmd)
        {
            Recipe recipe = cmd.ToRecipe();
            _dbContext.Add(recipe);
            await _dbContext.SaveChangesAsync();
            return recipe.RecipeId;
        }

        // Обновление рецепта в базе данных.
        public async Task UpdateRecipe(RecipeUpdateCommand cmd)
        {
            Recipe? recipe = await _dbContext.Recipes.FindAsync(cmd.Id);

            if (recipe == null)
                throw new Exception("Не удалось найти рецепт.");
            if (recipe.IsDeleted)
                throw new Exception("Невозможно обновить удаленный рецепт.");

            cmd.UpdateRecipe(recipe);
            await _dbContext.SaveChangesAsync();
        }

        // Удаление рецепта из базы данных.
        public async Task DeleteRecipe(int id)
        {
            Recipe? recipe = await _dbContext.Recipes.FindAsync(id);
            if (recipe is not null)
            {
                recipe.IsDeleted = true;
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> IsAvailableForUpdate(int id) =>
            await _dbContext.Recipes.Where(r => r.RecipeId == id && !r.IsDeleted).AnyAsync();
    }
}
