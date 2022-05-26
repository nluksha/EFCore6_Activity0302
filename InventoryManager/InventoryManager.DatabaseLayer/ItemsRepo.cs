using AutoMapper;
using InventoryManager.Models.DTOs;
using InventoryManager.DbLibrary;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using InventoryManager.Models;
using System.Diagnostics;

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
        public List<Item> GetItems()
        {
            var items = context.Items
                .Include(x => x.Category)
                .AsEnumerable()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name)
                .ToList();

            return items;
        }

        public List<ItemDto> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
        {
            var items = context.Items
                .Include(x => x.Category)
                .Where(x => x.CreatedDate >= minDateValue && x.CreatedDate <= maxDateValue)
                .ProjectTo<ItemDto>(mapper.ConfigurationProvider)
                .ToList();

            return items;
        }

        public List<GetItemsForListingDto> GetItemsForListingFromProcedure()
        {
            return context.ItemsForLisitng
                .FromSqlRaw("EXECUTE dbo.GetItemsForListing")
                .ToList();
        }

        public List<GetItemsTotalValueDto> GetItemsTotalValue(bool isActive)
        {
            var isActiveParm = new SqlParameter("IsActive", isActive);

            return context.GetItemsTotalValues
                .FromSqlRaw("SELECT * FROM [dbo].[GetItemsTotalValue] (@IsActive)", isActiveParm)
                .ToList();
        }

        public List<FullItemDetailDto> GetItemsWithGenresAndCategories()
        {
            return context.FullItemDetailDtos
                .FromSqlRaw("SELECT * FROM [dbo].[vwFullItemDetails]")
                .AsEnumerable()
                .OrderBy(x => x.ItemName)
                .ThenBy(x => x.GenreName)
                .ThenBy(x => x.Category)
                .ThenBy(x => x.PlayerName)
                .ToList();
        }

        public int UpsertItem(Item item)
        {
            if (item.Id > 0)
            {
                return UpdateItem(item);
            }
            else
            {
                return CreateItem(item);
            }
        }


        public void UpsertItems(List<Item> items)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in items)
                    {
                        var success = UpsertItem(item) > 0;

                        if (!success)
                        {
                            throw new Exception($"Error saving the item {item.Name}");
                        }
                    }

                    transaction.Commit();
                } 
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    transaction.Rollback();

                    throw;
                }
            }
        }

        public void DeleteItem(int id)
        {
            var item  = context.Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return;
            }

            item.IsDeleted = true;
            context.SaveChanges();
        }

        public void DeteleItems(List<int> itemIds)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var id in itemIds)
                    {
                        DeleteItem(id);
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    // log it:
                    Debug.WriteLine(ex.ToString());
                    transaction.Rollback();

                    throw;
                }
            }
        }

        private int CreateItem(Item item)
        {
            context.Items.Add(item);
            context.SaveChanges();

            var newItem = context.Items.FirstOrDefault(x => x.Name.ToLower().Equals(item.Name.ToLower()));

            if (newItem == null)
            {
                throw new Exception("Could not Create the item as expected");
            }

            return newItem.Id;
        }

        private int UpdateItem(Item item)
        {
            var dbItem = context.Items
                .Include(x => x.Category)
                .Include(x => x.ItemGenres)
                .Include(x => x.Players)
                .FirstOrDefault(x => x.Id == item.Id);

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
            
            context.SaveChanges();

            return item.Id;
        }

    }
}