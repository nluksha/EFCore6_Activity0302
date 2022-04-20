using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryManager.DbLibrary
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot configuration;

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
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("InventoryManager");

                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}