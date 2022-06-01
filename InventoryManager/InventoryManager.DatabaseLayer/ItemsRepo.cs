using AutoMapper;
using InventoryManager.Models.DTOs;
using InventoryManager.DbLibrary;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using InventoryManager.Models;
using System.Diagnostics;
using System.Transactions;
using System.Threading.Tasks;

namespace InventoryManager.DatabaseLayer
{
    public class ItemsRepo : IItemsRepo
    {
        private readonly IMapper mapper;
        private readonly InventoryDbContext context;

        public ItemsRepo(InventoryDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            this.context = context;
        }

        /*
        public List<ItemDto> GetItems()
        {
            var items = context.Items
                .ProjectTo<ItemDto>(mapper.ConfigurationProvider)
                .ToList();

            return items;
        }
        */
        public async Task<List<Item>> GetItems()
        {
            var items = await context.Items
                .Include(x => x.Category)
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return items;
        }

        public async Task<List<ItemDto>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
        {
            return await context.Items
                .Include(x => x.Category)
                .Where(x => x.CreatedDate >= minDateValue && x.CreatedDate <= maxDateValue)
                .ProjectTo<ItemDto>(mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<List<GetItemsForListingDto>> GetItemsForListingFromProcedure()
        {
            return await context.ItemsForLisitng
                .FromSqlRaw("EXECUTE dbo.GetItemsForListing")
                .ToListAsync();
        }

        public async Task<List<GetItemsTotalValueDto>> GetItemsTotalValue(bool isActive)
        {
            var isActiveParm = new SqlParameter("IsActive", isActive);

            return await context.GetItemsTotalValues
                .FromSqlRaw("SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParm)
                .ToListAsync();
        }

        public async Task<List<FullItemDetailDto>> GetItemsWithGenresAndCategories()
        {
            return await context.FullItemDetailDtos
                .FromSqlRaw("SELECT * FROM [dbo].[vwFullItemDetails]")
                .OrderBy(x => x.ItemName)
                .ThenBy(x => x.GenreName)
                .ThenBy(x => x.Category)
                .ThenBy(x => x.PlayerName)
                .ToListAsync();
        }

        public async Task<int> UpsertItem(Item item)
        {
            if (item.Id > 0)
            {
                return await UpdateItem(item);
            }
            else
            {
                return await CreateItem(item);
            }
        }

        private async Task<int> CreateItem(Item item)
        {
            await context.Items.AddAsync(item);
            await context.SaveChangesAsync();

            var newItem = await context.Items
                .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(item.Name.ToLower()));

            if (newItem == null)
            {
                throw new Exception("Could not Create the item as expected");
            }

            return newItem.Id;
        }

        private async Task<int> UpdateItem(Item item)
        {
            var dbItem = await context.Items
                .Include(x => x.Category)
                .Include(x => x.ItemGenres)
                .Include(x => x.Players)
                .FirstOrDefaultAsync(x => x.Id == item.Id);

            if (dbItem == null)
            {
                throw new Exception("Item not found");
            }

            dbItem.CategoryId = item.CategoryId;
            dbItem.CurrentOrFinalPrice = item.CurrentOrFinalPrice;
            dbItem.Description = item.Description;
            dbItem.IsActive = item.IsActive;
            dbItem.IsDeleted = item.IsDeleted;
            dbItem.IsOnSale = item.IsOnSale;

            if (item.ItemGenres != null)
            {
                dbItem.ItemGenres = item.ItemGenres;
            }

            dbItem.Name = item.Name;
            dbItem.Notes = item.Notes;

            if (item.Players != null)
            {
                dbItem.Players = item.Players;
            }

            dbItem.PurchasedDate = item.PurchasedDate;
            dbItem.PurchasePrice = item.PurchasePrice;
            dbItem.Quantity = item.Quantity;
            dbItem.SoldDate = item.SoldDate;

            await context.SaveChangesAsync();

            return item.Id;
        }

        public async Task UpsertItems(List<Item> items)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                try
                {
                    foreach (var item in items)
                    {
                        var success = await UpsertItem(item) > 0;

                        if (!success)
                        {
                            throw new Exception($"Error saving the item {item.Name}");
                        }
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        public async Task DeteleItems(List<int> itemIds)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
            {
                try
                {
                    foreach (var id in itemIds)
                    {
                        await DeleteItem(id);
                    }

                    scope.Complete();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());

                    throw;
                }
            }
        }

        public async Task DeleteItem(int id)
        {
            var item = await context.Items.FirstOrDefaultAsync(x => x.Id == id);

            if (item == null)
            {
                return;
            }

            item.IsDeleted = true;
            await context.SaveChangesAsync();
        }
    }
}