using InventoryManager;
using InventoryManager.DbLibrary;
using InventoryManager.Helpers;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration;
DbContextOptionsBuilder<InventoryDbContext> optionsBuilder;

BuildOptions();
EnsureItems();
ListInventory();

void BuildOptions()
{
    configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
    optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("InventoryManager"));
}

void EnsureItems()
{
    EnsureItem("Batman Begins");
    EnsureItem("Inception");
    EnsureItem("Remender the Titans");
    EnsureItem("Star Wars");
    EnsureItem("Top Gun");
}

void EnsureItem(string name)
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var existingItem = db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        if (existingItem == null)
        {
            var item = new Item { Name = name };

            db.Items.Add(item);
            db.SaveChanges();
        }
    }
}

void ListInventory()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = db.Items.OrderBy(x => x.Name).ToList();
        items.ForEach(x => Console.WriteLine($"New Items: {x.Name}"));
    }
}