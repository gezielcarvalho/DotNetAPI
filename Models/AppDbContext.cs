using Microsoft.EntityFrameworkCore;

namespace DotNetAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        // Add other DbSet properties for your entities here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity models and relationships here
        }
    }
}
