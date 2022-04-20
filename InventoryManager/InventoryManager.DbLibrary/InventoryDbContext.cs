using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.DbLibrary
{
    public class InventoryDbContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public InventoryDbContext()
        {
        }

        public InventoryDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=localhost; Initial Catalog=InventoryManager; Trusted_Connection=True");
            }
        }
    }
}