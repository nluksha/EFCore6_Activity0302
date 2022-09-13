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
using System.Transactions;

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

List<CategoryDto> categories = new List<CategoryDto>();

BuildOptions();
BuildMapper();

using (var db = new InventoryDbContext(optionsBuilder.Options))
{
    itemsService = new ItemsService(db, mapper);
    categoriesService = new CategoriesService(db, mapper);

    //await ListInventory();
    //await GetItemsForListing();
    //await GetAllActiveItemsAsPipeDelimitedString();
    //await GetItemsTotalValues();
    //await GetFullItemsDetails();
    //await GetItemsForListingLinq();
    //await ListCategoriesAndColors();

    //await ExploreManyToManyRelationships(db);
    //await EnsureItemsHaveGenres(db);
    //await DemonstateSplitQueries(db);
    //await DemoSimpleLogging(db);


    /*
    // Insert
    Console.WriteLine("Would you like to create items?");
    var createItems = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    if (createItems)
    {
        Console.WriteLine("Adding new Item(s)");
        await CreateMultipleItems();
        Console.WriteLine("Items added");

        var inventory = await itemsService.GetItems();
        inventory.ForEach(x => Console.WriteLine($"Item: {x}"));
    }

    // Update
    Console.WriteLine("Would you like tu update items?");
    var updateItems = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    if (updateItems)
    {
        Console.WriteLine("Update Items(s)");
        await UpdateMultipleItems();
        Console.WriteLine("Items updated");

        var inventory2 = await itemsService.GetItems();
        inventory2.ForEach(x => Console.WriteLine($"Item: {x}"));
    }

    // Delete
    Console.WriteLine("Would you like tu delete items?");
    var deleteItems = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    if (deleteItems)
    {
        Console.WriteLine("Deleting Items(s)");
        await DeleteMultipleItems();
        Console.WriteLine("Items deleted");

        var inventory3 = await itemsService.GetItems();
        inventory3.ForEach(x => Console.WriteLine($"Item: {x}"));
    }

    Console.WriteLine("Program Completed");
    */

}

async Task DemoSimpleLogging(InventoryDbContext db)
{
    var fullItemDetails = await db.Items
        .Include(x => x.Players)
        .Include(x => x.ItemGenres).ThenInclude(y => y.Genre)
        .Include(x => x.Category)
        .Where(x => x.IsActive && !x.IsDeleted)
        .AsNoTracking()
        .ToListAsync();

    var outputItems = mapper.Map<List<ItemDto>>(fullItemDetails);
    foreach (var item in outputItems)
    {
        Console.WriteLine($"NEW Item : {item}");
    }
}

async Task DemonstateSplitQueries(InventoryDbContext db)
{
    using (var scope = new TransactionScope(TransactionScopeOption.Required,
        new TransactionOptions { IsolationLevel = IsolationLevel.Serializable },
        TransactionScopeAsyncFlowOption.Enabled))
    {
        var fullItemDetails = await db.Items
            .Include(x => x.Players)
            .Include(x => x.ItemGenres).ThenInclude(y => y.Genre)
            .Include(x => x.Category)
            .Where(x => x.IsActive && !x.IsDeleted)
            .AsSplitQuery()
            .AsNoTracking()
            .ToListAsync();

        var outputItems = mapper.Map<List<ItemDto>>(fullItemDetails);
        foreach (var item in outputItems)
        {
            Console.WriteLine($"NEW Item : {item}");
        }
    }
}

async Task EnsureItemsHaveGenres(InventoryDbContext db)
{
    Console.WriteLine(new String('*', 80));
    Console.WriteLine(new String('*', 80));
    var nonFilteredItems = await db.Items.AsNoTracking().Include(x => x.Players).ToListAsync();
    var allPlayers = new List<Player>();

    foreach (var item in nonFilteredItems)
    {
        foreach (var player in item.Players)
        {
            allPlayers.Add(player);
        }
    }

    Console.WriteLine("Non-filtered Items:");
    allPlayers.ForEach(x => Console.WriteLine(x.Name));

    Console.WriteLine(new String('*', 80));
    Console.WriteLine(new String('*', 80));
    var filteredItems = await db.Items
        .AsNoTracking()
        .Include(
        item => item.Players.Where(
            p => p.Name.Contains("ar")
            )
        ).ToListAsync();

    var filteredPlayers = new List<Player>();
    foreach (var fi in filteredItems)
    {
        foreach (var player in fi.Players)
        {
            filteredPlayers.Add(player);
        }
    }

    Console.WriteLine("Filtered Players");
    filteredPlayers.ForEach(x => Console.WriteLine(x.Name));

    Console.WriteLine(new String('*', 80));
    Console.WriteLine(new String('*', 80));
    var selectFilteredItems = await db.Items.AsNoTracking()
        .Select(item => new
        {
            Id = item.Id,
            Name = item.Name,
            Players = item.Players
        .Where(p => p.Name.Contains("ar"))
        .Select(pl => new Player
        {
            Id = pl.Id,
            Name = pl.Name
        }).ToList()
        }).ToListAsync();

    var selectedFilteredPlayers = new List<Player>();
    foreach (var fi in selectFilteredItems)
    {
        foreach (var player in fi.Players)
        {
            selectedFilteredPlayers.Add(player);
        }
    }

    Console.WriteLine("Selected [Projected] Filtered Players");
    selectedFilteredPlayers.ForEach(x => Console.WriteLine(x.Name));
}

async Task ExploreManyToManyRelationships(InventoryDbContext db)
{
    var items = await db.Items
        .Include(i => i.Players)
        .Include(i => i.ItemGenres).ThenInclude(ig => ig.Genre)
        .Where(i => !i.IsDeleted && i.IsActive)
        .ToListAsync();

    foreach (var item in items)
    {
        Console.WriteLine($"New Item: {item.Name} found...");

        foreach (var itemGenre in item.ItemGenres)
        {
            Console.WriteLine($"Item {item.Name} has genre {itemGenre.Genre.Name}");
        }

        foreach (var player in item.Players)
        {
            Console.WriteLine($"Item {item.Name} has player {player.Name}");
        }
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

async Task ListInventory()
{
    var res = await itemsService.GetItems();
    res.ForEach(x => Console.WriteLine($"New Item: {x}"));
}

async Task DeleteAllItems()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = await db.Items.ToListAsync();
        db.Items.RemoveRange(items);

        await db.SaveChangesAsync();
    }
}

async Task UpdateItems()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = await db.Items.ToListAsync();
        foreach (var item in items)
        {
            item.CurrentOrFinalPrice = 9.99M;
        }

        db.Items.UpdateRange(items);
        await db.SaveChangesAsync();
    }
}

async Task GetItemsForListing()
{
    var res = await itemsService.GetItemsForListingFromProcedure();

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

async Task GetAllActiveItemsAsPipeDelimitedString()
{
    Console.WriteLine($"All ctive Items: {await itemsService.GetAllItemsPipeDelimitedString()}");
}

async Task GetItemsTotalValues()
{
    var res = await itemsService.GetItemsTotalValue(true);

    foreach (var item in res)
    {
        Console.WriteLine($"New ITem {item.Id,-10} | {item.Name,-50} | {item.Quantity,-4} | {item.TotalValue,-5}");
    }
}

async Task GetFullItemsDetails()
{
    var res = await itemsService.GetItemsWithGenresAndCategories();

    foreach (var item in res)
    {
        Console.WriteLine($"New ITem {item.Id,-10} | {item.ItemName,-50} | {item.Category,-4} | {item.GenreName,-5}");
    }
}

async Task GetItemsForListingLinq()
{
    var minDateValue = new DateTime(2021, 1, 1);
    var maxDateValue = new DateTime(2024, 1, 1);

    var res = await itemsService.GetItemsByDateRange(minDateValue, maxDateValue);

    foreach (var item in res.OrderBy(y => y.CategoryName).ThenBy(z => z.Name))
    {
        Console.WriteLine(item);
    }
}

async Task ListCategoriesAndColors()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var res = await categoriesService.ListCategoriesAndDetails();
        categories = res;

        res.ForEach(x => Console.WriteLine($"Category: {x.Category} is {x.CategoryDetail.Color}"));
    }
}

async Task CreateMultipleItems()
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
            await itemsService.UpsertItem(newItem);
        }
        else
        {
            allItems.Add(newItem);
        }

        Console.WriteLine("Would you like to create another items?");
        createAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

        if (batchCreate && !createAnother)
        {
            await itemsService.UpsertItems(allItems);
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

async Task UpdateMultipleItems()
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

        var items = await itemsService.GetItems();
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
                    await itemsService.UpsertItem(updItem);
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
            await itemsService.UpsertItems(allItems);
        }
    }
}

async Task DeleteMultipleItems()
{
    Console.WriteLine("Would you like to delete items as a batch?");
    bool batchDelete = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

    var allItems = new List<int>(); 
    bool deleteAnother = true; 
    
    while (deleteAnother == true)
    {
        Console.WriteLine("Items");
        Console.WriteLine("Enter the ID number to delete"); 
        Console.WriteLine("*******************************"); 

        var items = await itemsService.GetItems();
        items.ForEach(x => Console.WriteLine($"ID: {x.Id} | {x.Name}"));

        Console.WriteLine("*******************************"); 

        if (batchDelete && allItems.Any())
        {
            Console.WriteLine("Items scheduled for delete"); 
            allItems.ForEach(x => Console.Write($"{x},"));
            Console.WriteLine(); 
            Console.WriteLine("*******************************");
        }

        int id = 0;

        if (int.TryParse(Console.ReadLine(), out id))
        {
            var itemMatch = items.FirstOrDefault(x => x.Id == id);
            if (itemMatch != null)
            {
                if (batchDelete)
                {
                    if (!allItems.Contains(itemMatch.Id))
                    {
                        allItems.Add(itemMatch.Id);
                    }
                }
                else
                {
                    Console.WriteLine($"Are you sure you want to delete the item {itemMatch.Id}-{itemMatch.Name}");
                    if (Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase))
                    {
                        await itemsService.DeleteItem(itemMatch.Id); 
                        Console.WriteLine("Item Deleted");
                    }
                }
            }
        }

        Console.WriteLine("Would you like to delete another item?");
        deleteAnother = Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase);

        if (batchDelete && !deleteAnother)
        {
            Console.WriteLine("Are you sure you want to delete the following items: ");
            allItems.ForEach(x => Console.Write($"{x},"));
            Console.WriteLine();

            if (Console.ReadLine().StartsWith("y", StringComparison.OrdinalIgnoreCase))
            {
                await itemsService.DeleteItems(allItems); Console.WriteLine("Items Deleted");
            }
        }
    }
}