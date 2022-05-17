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

IConfigurationRoot configuration;
DbContextOptionsBuilder<InventoryDbContext> optionsBuilder;
const string systenUserId = "9164f960-7946-487a-aa77-c46e9a403568";
const string loggedInUserId = "cf1ef43f-2e84-4639-a2de-038f66f06cda";

//AutoMapper
MapperConfiguration mapperConfig;
IMapper mapper;
IServiceProvider serviceProvider;

BuildOptions();
BuildMapper();

// DeleteAllItems();

// Moved to Migrator
// EnsureItems();
// UpdateItems();

//ListInventory();
//GetItemsForListing();
GetAllActiveItemsAsPipeDelimitedString();
//GetItemsTotalValues();
//GetItemsForListingLinq();
//ListInventoryWithProjection();
//ListCategoriesAndColors();

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
            var item = new Item { 
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
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = db.Items.OrderBy(x => x.Name).Take(20)
            .Select(x => new ItemDto
            {
                Name = x.Name,
                Description = x.Description
            })
            .ToList();

        // var result = mapper.Map<List<Item>, List<ItemDto>>(items);

        items.ForEach(x => Console.WriteLine($"New Item: {x}"));
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

void GetItemsForListing()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var res = db.ItemsForLisitng.FromSqlRaw("EXECUTE dbo.GetItemsForListing").ToList();

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
}

void GetAllActiveItemsAsPipeDelimitedString()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        //var isActiveParm = new SqlParameter("IsActive", 1);
        //var res = db.AllItemsOutput.FromSqlRaw("SELECT [dbo].[ItemNamesPipeDeliminatedString] (@IsActive) AllItems", isActiveParm).FirstOrDefault();

        var result = db.Items.Where(x => x.IsActive).Select(x => x.Name).ToList();
        var pipeDelimitedString = string.Join("| ", result);

        Console.WriteLine($"All ctive Items: {pipeDelimitedString}");
    }
}

void GetItemsTotalValues()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var isActiveParm = new SqlParameter("IsActive", 1);

        var res = db.GetItemsTotalValues.FromSqlRaw("SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParm).ToList();

        foreach (var item in res)
        {
            Console.WriteLine($"New ITem {item.Id, -10} | {item.Name, -50} | {item.Quantity, -4} | {item.TotalValue, -5}");
        }
    }
}

void GetItemsForListingLinq()
{
    var minDateValue = new DateTime(2021, 1, 1);
    var maxDateValue = new DateTime(2024, 1, 1);

    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var res = db.Items
            .Include(x => x.Category).ToList() // for fixing encripting essue
            .Select(x => new ItemDto
        {
            CreatedDate = x.CreatedDate,
            CategoryName = x.Category.Name,
            Description = x.Description,
            IsActive = x.IsActive,
            IsDeleted = x.IsDeleted,
            Name = x.Name,
            Notes = x.Notes,
            CategoryId = x.Category.Id,
            Id = x.Id
        })
            .Where(x => x.CreatedDate >= minDateValue && x.CreatedDate <= maxDateValue)
            .OrderBy(y => y.CategoryName)
            .ThenBy(z => z.Name)
            .ToList();

        foreach (var item in res)
        {
            Console.WriteLine(item);
        }
    }
}

void ListInventoryWithProjection()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var items = db.Items
            //.OrderBy(x => x.Name)
            .ProjectTo<ItemDto>(mapper.ConfigurationProvider)
            .ToList();

        items.OrderBy(x => x.Name).ToList()
            .ForEach(x => Console.WriteLine($"New Item: {x}"));
    }
}

void ListCategoriesAndColors()
{
    using (var db = new InventoryDbContext(optionsBuilder.Options))
    {
        var res = db.Categories
            .Include(x => x.CategoryDetail)
            .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
            .ToList();

        res.ForEach(x => Console.WriteLine($"Category: {x.Category} is {x.CategoryDetail.Color}"));
    }

}