using Microsoft.EntityFrameworkCore;

namespace WebApiFiltersApp.Data
{
    // Тип DbContext является реализацией паттернов "Единица работы" и "Репозиторий".
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Вы будете использовать свойство Recipes для выполнения запроса к базе данных.
        // Мы не указали класс Ingredient в классе AppDbContext, но он будет смоделирован EF Core, как он
        // предоставляется в Recipe.
        public DbSet<Recipe> Recipes { get; set; }
    }
}
