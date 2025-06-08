using CatalogService.Models.DAO;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Data
{
    public class CatalogServiceContext : DbContext
    {
        public CatalogServiceContext(DbContextOptions<CatalogServiceContext> options)
            : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasAlternateKey(c => c.Name);

        }
    }
}
