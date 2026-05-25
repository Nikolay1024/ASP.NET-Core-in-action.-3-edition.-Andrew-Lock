using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MinimalApiXunitTestingApp.Commands;
using MinimalApiXunitTestingApp.Data;
using MinimalApiXunitTestingApp.Services;
using MinimalApiXunitTestingApp.ViewModels;

namespace MinimalApiXunitTestingApp.Test.UnitTests
{
    public class RecipeServiceTest
    {
        [Fact]
        // Используя два отдельных экземпляра DbContext, можно избежать ошибок в тестах из-за кеширования данных
        // EF Core без их записи в БД.
        public async Task GetRecipeDetail_UsualCase()
        {
            // Настраивает соединение с SQLite в памяти с помощью специальной строки подключения.
            var connection = new SqliteConnection("DataSource=:memory:");

            // Открывает соединение, чтобы EF Core не закрывал его автоматически при освобождении DbContext.
            connection.Open();

            // Создает DbContextOptions<T> и настраивает его для использования соединения SQLite.
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            // Создает DbContext и передает DbContextOptions<T>, чтобы использовать уже открытое соединение.
            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                // Обеспечивает соответствие БД в памяти и модели EF Core (аналогично запуску миграций).
                dbContext.Database.EnsureCreated();

                // Добавляет несколько рецептов в DbContext.
                dbContext.Recipes.AddRange(
                    new Recipe()
                    {
                        Name = "Пельмени",
                        Method = "Варить",
                        TimeToCook = TimeSpan.FromMinutes(20),
                        Ingredients = new List<Ingredient>()
                        {
                            new() { Name = "Пельмени", Quantity = 20, Unit = "шт" },
                            new() { Name = "Вода", Quantity = 2, Unit = "литр" },
                        }
                    },
                    new Recipe()
                    {
                        Name = "Суп",
                        Method = "Варить",
                        TimeToCook = TimeSpan.FromHours(3),
                        Ingredients = new List<Ingredient>()
                        {
                            new() { Name = "Мясо", Quantity = 1, Unit = "кг" },
                            new() { Name = "Вода", Quantity = 5, Unit = "литр" },
                            new() { Name = "Картошка", Quantity = 1, Unit = "кг" },
                        }
                    });

                // Сохраняет изменения в БД в памяти.
                await dbContext.SaveChangesAsync();
            }

            // Создает новый DbContext для проверки возможности извлечения данных из DbContext.
            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var recipeService = new RecipeService(dbContext);

                // Метод GetRecipeDetail() выполняет запрос к БД в памяти.
                RecipeViewModel? recipe = await recipeService.GetRecipeDetail(2);

                // Проверяет, правильно ли вы получили рецепт из БД в памяти.
                Assert.NotNull(recipe);
                Assert.Equal(2, recipe.Id);
                Assert.Equal("Суп", recipe.Name);
            }
        }

        [Fact]
        public async Task GetRecipeDetail_ErrorCase()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                dbContext.Database.EnsureCreated();
                dbContext.Recipes.AddRange(
                    new Recipe()
                    {
                        Name = "Пельмени",
                        Method = "Варить",
                        TimeToCook = TimeSpan.FromMinutes(20),
                        Ingredients = new List<Ingredient>()
                        {
                            new() { Name = "Пельмени", Quantity = 20, Unit = "шт" },
                            new() { Name = "Вода", Quantity = 2, Unit = "литр" },
                        }
                    },
                    new Recipe()
                    {
                        Name = "Суп",
                        Method = "Варить",
                        TimeToCook = TimeSpan.FromHours(3),
                        Ingredients = new List<Ingredient>()
                        {
                            new() { Name = "Мясо", Quantity = 1, Unit = "кг" },
                            new() { Name = "Вода", Quantity = 5, Unit = "литр" },
                            new() { Name = "Картошка", Quantity = 1, Unit = "кг" },
                        },
                        IsDeleted = true
                    });

                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                var recipeService = new RecipeService(dbContext);

                RecipeViewModel? recipe = await recipeService.GetRecipeDetail(2);

                Assert.Null(recipe);
            }
        }

        [Fact]
        public async Task CreateRecipe_UsualCase()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            DbContextOptions<AppDbContext> dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(connection)
                .Options;

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                dbContext.Database.EnsureCreated();
                var recipeService = new RecipeService(dbContext);
                var cmd = new RecipeCreateCommand()
                {
                    Name = "Картошка с мясом",
                    Method = "Жарить",
                    TimeToCookMins = 30,
                    Ingredients = new List<IngredientCreateCommand>()
                    {
                        new() { Name = "Мясо", Quantity = 1, Unit = "кг" },
                        new() { Name = "Картошка", Quantity = 1, Unit = "кг" },
                    }
                };
                await recipeService.CreateRecipe(cmd);
            }

            using (var dbContext = new AppDbContext(dbContextOptions))
            {
                int cnt = await dbContext.Recipes.CountAsync();
                Recipe recipe = await dbContext.Recipes.SingleAsync();

                Assert.Equal(1, cnt);
                Assert.Equal("Картошка с мясом", recipe.Name);
            }
        }
    }
}
