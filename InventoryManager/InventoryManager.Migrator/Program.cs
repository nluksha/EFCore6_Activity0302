using InventoryManager;
using InventoryManager.DbLibrary;
using InventoryManager.Helpers;
using InventoryManager.Migrator;
using InventoryManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration;
DbContextOptionsBuilder<InventoryDbContext> optionsBuilder;

BuildOptions();
ApplyMigrations();
ExecuteCustomSeedData();

void BuildOptions()
{
    configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
    optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("InventoryManager"));
}

void ApplyMigrations()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        db.Database.Migrate();
    }
}

void ExecuteCustomSeedData()
{
    using (var context = new InventoryDbContext(optionsBuilder.Options))
    {
        var categories = new BuildCategories(context);
        categories.ExecuteSeed();
    }
}