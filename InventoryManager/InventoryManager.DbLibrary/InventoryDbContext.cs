using Microsoft.EntityFrameworkCore;

namespace InventoryManager.DbLibrary
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext()
        {
        }

        public InventoryDbContext(DbContextOptions options)
            : base(options)
        {
        }
    }
}