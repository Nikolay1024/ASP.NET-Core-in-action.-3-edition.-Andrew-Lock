using Microsoft.EntityFrameworkCore;

namespace MinimalApiBackgroundService2App.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Album> Albums { get; set; }
    }
}
