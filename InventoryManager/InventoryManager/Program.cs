﻿using InventoryManager;
using InventoryManager.DbLibrary;
using InventoryManager.Helpers;
using InventoryManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

IConfigurationRoot configuration;
DbContextOptionsBuilder<InventoryDbContext> optionsBuilder;
const string systenUserId = "9164f960-7946-487a-aa77-c46e9a403568";
const string loggedInUserId = "cf1ef43f-2e84-4639-a2de-038f66f06cda";

BuildOptions();

// DeleteAllItems();
EnsureItems();
UpdateItems();
ListInventory();

void BuildOptions()
{
    configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
    optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("InventoryManager"));
}

void EnsureItems()
{
    EnsureItem("Batman Begins", "You either die the hero...", "Christian Bale");
    EnsureItem("Inception", "You must not be afraid to dream...", "Leonardo DiCaprio");
    EnsureItem("Remender the Titans", "Left Side, strong side...", "Denzell Washington");
    EnsureItem("Star Wars", "He will join us or die...", "Harrison Ford");
    EnsureItem("Top Gun", "I feel the need, the need for speed...", "Tom Cruise");
}

void EnsureItem(string name, string description, string notes)
{
    var random = new Random();

    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var existingItem = db.Items.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

        if (existingItem == null)
        {
            var item = new Item { 
                Name = name,
                CreatedByUserId = loggedInUserId,
                IsActive = true,
                Quantity = random.Next(),
                Description = description,
                Notes = notes
            };

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

void DeleteAllItems()
{
    using(var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = db.Items.ToList();
        db.Items.RemoveRange(items);

        db.SaveChanges();
    }
}

void UpdateItems()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = db.Items.ToList();
        foreach (var item in items)
        {
            item.CurrentOrFinalPrice = 9.99M;
        }

        db.Items.UpdateRange(items);
        db.SaveChanges();
    }
}