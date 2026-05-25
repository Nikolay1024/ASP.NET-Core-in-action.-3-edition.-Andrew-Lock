using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity2App.Data
{
    // Identity предоставляет класс DbContext с именем IdentityDbContext, от которого можно наследовать. Базовый
    // класс IdentityDbContext включает в себя необходимый тип DbSet<T> для хранения пользовательских сущностей
    // с помощью EF Core.
    // Фактически, обновив таким образом базовый класс контекста, мы добавили всю загрузку новых сущностей
    // в модель данных EF Core.
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Recipe> Recipes { get; set; }
    }
}
