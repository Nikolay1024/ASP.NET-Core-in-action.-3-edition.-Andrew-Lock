using Microsoft.EntityFrameworkCore;

namespace WorkerServiceApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Album> Albums { get; set; }
    }
}
