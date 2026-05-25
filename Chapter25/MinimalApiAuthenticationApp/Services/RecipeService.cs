using Microsoft.EntityFrameworkCore;
using MinimalApiAuthenticationApp.Commands;
using MinimalApiAuthenticationApp.Data;
using MinimalApiAuthenticationApp.ViewModels;
using System.Security.Claims;

namespace MinimalApiAuthenticationApp.Services
{
    public class RecipeService
    {
        readonly AppDbContext _context;

        // Экземпляр AppDbContext передается в конструктор класса с помощь внедрения зависимостей.
        public RecipeService(AppDbContext context) => _context = context;

        // Получение сводки рецептов из базы данных.
        public async Task<List<RecipeSummaryViewModel>> GetRecipes()
        {
            // EF Core будет запрашивать только те столбцы рецепта, которые необходимы для
            // правильного отображения модели представления (RecipeId, Name, TimeToCook).
            // ToListAsync() выполняет SQL-запрос и создает окончательную модель представления.
            return await _context.Recipes.Where(r => !r.IsDeleted)
                .Select(r => new RecipeSummaryViewModel()
                {
                    Id = r.RecipeId,
                    Name = r.Name,
                    TimeToCook = r.TimeToCook.ToString(@"hh\:mm"),
                    NumberOfIngredients = r.Ingredients.Count,
                })
                .ToListAsync();
        }

        public async Task<Recipe?> GetRecipe(int id)
        {
            return await _context.Recipes.Where(r => r.RecipeId == id).SingleOrDefaultAsync();
        }

        // Получение рецепта по идентификатору из базы данных.
        public async Task<RecipeViewModel?> GetRecipeDetail(int id)
        {
            // SingleOrDefaultAsync() выполняет SQL-запрос и сопоставляет данные с моделью представления.
            return await _context.Recipes.Where(r => r.RecipeId == id && !r.IsDeleted)
                .Select(r => new RecipeViewModel()
                {
                    Id = r.RecipeId,
                    Name = r.Name,
                    Method = r.Method,
                    LastModified = r.LastModified,
                    // Загружает и сопоставляет связанные ингредиенты как часть одного SQL-запроса.
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
        public async Task<int> CreateRecipe(RecipeCreateCommand cmd, ClaimsPrincipal createdBy)
        {
            Recipe recipe = cmd.ToRecipe(createdBy);
            // Сообщаем EF Core, что нужно отслеживать новые сущности.
            _context.Add(recipe);
            // Даем EF Core указание вести запись сущностей в базу данных, используя асинхронную команду.
            await _context.SaveChangesAsync();
            // EF Core заполняет поле RecipeId в новом рецепте при его сохранении.
            return recipe.RecipeId;
        }

        // Обновление рецепта в базе данных.
        public async Task UpdateRecipe(RecipeUpdateCommand cmd)
        {
            // Метод Find() проверяет, не отслеживается ли сущность в DbContext. Если это так то сущность возвращается
            // немедленно без обращения к базе данных.
            Recipe? recipe = await _context.Recipes.FindAsync(cmd.Id);

            if (recipe == null)
                throw new Exception("Не удалось найти рецепт.");
            if (recipe.IsDeleted)
                throw new Exception("Невозможно обновить удаленный рецепт.");

            // Устанавливает новые значения для сущности Recipe.
            cmd.UpdateRecipe(recipe);
            // Выполняет SQL для сохранения изменений в базе данных.
            await _context.SaveChangesAsync();
        }

        // Удаление рецепта из базы данных.
        public async Task DeleteRecipe(int id)
        {
            Recipe? recipe = await _context.Recipes.FindAsync(id);
            if (recipe is not null)
            {
                recipe.IsDeleted = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsAvailableForManage(int id)
        {
            return await _context.Recipes.Where(r => r.RecipeId == id && !r.IsDeleted).AnyAsync();
        }
    }
}
