using InventoryManager;
using InventoryManager.DbLibrary;
using InventoryManager.Helpers;
using InventoryManager.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using InventoryManager.Models.DTOs;
using AutoMapper.QueryableExtensions;
using InventoryManager.BusinessLayer;

IConfigurationRoot configuration;
DbContextOptionsBuilder<InventoryDbContext> optionsBuilder;
const string systenUserId = "9164f960-7946-487a-aa77-c46e9a403568";
const string loggedInUserId = "cf1ef43f-2e84-4639-a2de-038f66f06cda";

IItemsService itemsService;
ICategoriesService categoriesService;

//AutoMapper
MapperConfiguration mapperConfig;
IMapper mapper;
IServiceProvider serviceProvider;

List<CategoryDto> categories;

BuildOptions();
BuildMapper();

using (var db = new InventoryDbContext(optionsBuilder.Options))
{
    itemsService = new ItemsService(db, mapper);
    categoriesService = new CategoriesService(db, mapper);

    ListInventory();
    GetItemsForListing();
    GetAllActiveItemsAsPipeDelimitedString();
    GetItemsTotalValues();
    GetFullItemsDetails();
    GetItemsForListingLinq();
    ListCategoriesAndColors();

    // Insert
    Console.WriteLine("Would you like to create items?");
    var createItems = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    if (createItems)
    {
        Console.WriteLine("Adding new Item(s)");
        CreateMultipleItems();
        Console.WriteLine("Items added");

        var inventory = itemsService.GetItems();
        inventory.ForEach(x => Console.WriteLine($"Item: {x}"));
    }

    // Update
    Console.WriteLine("Would you like tu update items?");
    var updateItems = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    if (updateItems)
    {
        Console.WriteLine("Update Items(s)");
        UpdateMultipleItems();
        Console.WriteLine("Items updated");

        var inventory2 = itemsService.GetItems();
        inventory2.ForEach(x => Console.WriteLine($"Item: {x}"));
    }

}

void BuildOptions()
{
    configuration = ConfigurationBuilderSingleton.ConfigurationRoot;
    optionsBuilder = new DbContextOptionsBuilder<InventoryDbContext>();
    optionsBuilder.UseSqlServer(configuration.GetConnectionString("InventoryManager"));
}

void BuildMapper()
{
    var services = new ServiceCollection();
    services.AddAutoMapper(typeof(InventoryMapper));
    serviceProvider = services.BuildServiceProvider();

    mapperConfig = new MapperConfiguration(cfg =>
    {
        cfg.AddProfile<InventoryMapper>();
    });
    mapperConfig.AssertConfigurationIsValid();
    mapper = mapperConfig.CreateMapper();
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
            var item = new Item
            {
                Name = name,
                CreatedByUserId = loggedInUserId,
                IsActive = true,
                Quantity = random.Next(1, 1000),
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
    var res = itemsService.GetItems();
    res.ForEach(x => Console.WriteLine($"New Item: {x}"));
}

void DeleteAllItems()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
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

void GetItemsForListing()
{
    var res = itemsService.GetItemsForListingFromProcedure();

    foreach (var item in res)
    {
        var output = $"ITEM {item.Name} {item.Description}";

        if (!string.IsNullOrEmpty(item.CategoryName))
        {
            output = $"{output} has category: {item.CategoryName}";
        }
        Console.WriteLine(output);
    }
}

void GetAllActiveItemsAsPipeDelimitedString()
{
    Console.WriteLine($"All ctive Items: {itemsService.GetAllItemsPipeDelimitedString()}");
}

void GetItemsTotalValues()
{
    var res = itemsService.GetItemsTotalValue(true);

    foreach (var item in res)
    {
        Console.WriteLine($"New ITem {item.Id,-10} | {item.Name,-50} | {item.Quantity,-4} | {item.TotalValue,-5}");
    }
}

void GetFullItemsDetails()
{
    var res = itemsService.GetItemsWithGenresAndCategories();

    foreach (var item in res)
    {
        Console.WriteLine($"New ITem {item.Id,-10} | {item.ItemName,-50} | {item.Category,-4} | {item.GenreName,-5}");
    }
}

void GetItemsForListingLinq()
{
    var minDateValue = new DateTime(2021, 1, 1);
    var maxDateValue = new DateTime(2024, 1, 1);

    var res = itemsService.GetItemsByDateRange(minDateValue, maxDateValue)
        .OrderBy(y => y.CategoryName)
        .ThenBy(z => z.Name);

    foreach (var item in res)
    {
        Console.WriteLine(item);
    }
}

void ListCategoriesAndColors()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var res = categoriesService.ListCategoriesAndDetails();
        categories = res;

        res.ForEach(x => Console.WriteLine($"Category: {x.Category} is {x.CategoryDetail.Color}"));
    }
}

void CreateMultipleItems()
{
    Console.WriteLine("Would you like to create items as a batch?");
    var batchCreate = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);
    var allItems = new List<CreateOrUpdateItemDto>();

    bool createAnother = true;
    while (createAnother)
    {
        var newItem = new CreateOrUpdateItemDto();
        Console.WriteLine("Createing a new item.");

        Console.WriteLine("Please enter the Name");
        newItem.Name = Console.ReadLine();

        Console.WriteLine("Please enter the Description");
        newItem.Description = Console.ReadLine();

        Console.WriteLine("Please enter the Notes");
        newItem.Notes = Console.ReadLine();

        Console.WriteLine("Please enter the Category [B]ooks, [M]ovies, [G]ames;");
        newItem.CategoryId = GetCategoryId(Console.ReadLine().Substring(0, 1).ToUpper());

        if (!batchCreate)
        {
            itemsService.UpsertItem(newItem);
        }
        else
        {
            allItems.Add(newItem);
        }

        Console.WriteLine("Would you like to create another items?");
        createAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

        if (batchCreate && !createAnother)
        {
            itemsService.UpsertItems(allItems);
        }
    }
}

int GetCategoryId(string input)
{
    switch (input)
    {
        case "B":
            return categories.FirstOrDefault(x => x.Category.ToLower().Equals("books"))?.Id ?? -1;
        case "M":
            return categories.FirstOrDefault(x => x.Category.ToLower().Equals("movies"))?.Id ?? -1;
        case "G":
            return categories.FirstOrDefault(x => x.Category.ToLower().Equals("games"))?.Id ?? -1;
        default:
            return -1;
    }
}

void UpdateMultipleItems()
{
    Console.WriteLine("Would you like to update items as a batch?");
    bool batchUpdate = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    var allItems = new List<CreateOrUpdateItemDto>();
    bool updateAnother = true;

    while (updateAnother == true)
    {
        Console.WriteLine("Items");
        Console.WriteLine("Enter the ID number to update");
        Console.WriteLine("*******************************");

        var items = itemsService.GetItems();
        items.ForEach(x => Console.WriteLine($"ID: {x.Id} | {x.Name}"));

        Console.WriteLine("*******************************");

        int id = 0;
        if (int.TryParse(Console.ReadLine(), out id))
        {
            var itemMatch = items.FirstOrDefault(x => x.Id == id);

            if (itemMatch != null)
            {
                var updItem = mapper.Map<CreateOrUpdateItemDto>(mapper.Map<Item>(itemMatch));

                Console.WriteLine("Enter the new name [leave blank to keep existing]");
                var newName = Console.ReadLine();
                updItem.Name = !string.IsNullOrWhiteSpace(newName) ? newName : updItem.Name;

                Console.WriteLine("Enter the new desc [leave blank to keep existing]");
                var newDesc = Console.ReadLine();
                updItem.Description = !string.IsNullOrWhiteSpace(newDesc) ? newDesc : updItem.Description;

                Console.WriteLine("Enter the new notes [leave blank to keep existing]");
                var newNotes = Console.ReadLine();
                updItem.Notes = !string.IsNullOrWhiteSpace(newNotes) ? newNotes : updItem.Notes;

                Console.WriteLine("Toggle Item Active Status? [y/n]");
                var toggleActive = Console.ReadLine().Substring(0, 1).Equals("y", StringComparison.OrdinalIgnoreCase);

                if (toggleActive)
                {
                    updItem.IsActive = !updItem.IsActive;
                }

                Console.WriteLine("Enter the category - [B]ooks, [M]ovies, [G]ames, or [N]o Change");
                var userChoice = Console.ReadLine().Substring(0, 1).ToUpper();
                updItem.CategoryId = userChoice.Equals("N", StringComparison.OrdinalIgnoreCase) ? itemMatch.CategoryId : GetCategoryId(userChoice);

                if (!batchUpdate)
                {
                    itemsService.UpsertItem(updItem);
                }
                else
                {
                    allItems.Add(updItem);
                }
            }
        }

        Console.WriteLine("Would you like to update another?");
        updateAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

        if (batchUpdate && !updateAnother)
        {
            itemsService.UpsertItems(allItems);
        }
    }
}
