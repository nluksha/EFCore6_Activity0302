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
    }
}