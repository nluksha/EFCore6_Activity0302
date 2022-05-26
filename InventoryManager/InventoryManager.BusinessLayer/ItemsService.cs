using AutoMapper;
using InventoryManager.DatabaseLayer;
using InventoryManager.DbLibrary;
using InventoryManager.Models;
using InventoryManager.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.BusinessLayer
{
    public class ItemsService : IItemsService
    {
        private readonly IItemsRepo dbRepo;
        private readonly IMapper mapper;

        public ItemsService(InventoryDbContext context, IMapper mapper)
        {
            this.mapper = mapper;
            dbRepo = new ItemsRepo(context, mapper);
        }

        public string GetAllItemsPipeDelimitedString()
        {
            var items = GetItems();

            return String.Join('|', items);
        }

        public List<ItemDto> GetItems()
        {
            return mapper.Map<List<ItemDto>>(dbRepo.GetItems());
        }

        public List<ItemDto> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
        {
            return dbRepo.GetItemsByDateRange(minDateValue, maxDateValue);
        }

        public List<GetItemsForListingDto> GetItemsForListingFromProcedure()
        {
            return dbRepo.GetItemsForListingFromProcedure();
        }

        public List<GetItemsTotalValueDto> GetItemsTotalValue(bool isActive)
        {
            return dbRepo.GetItemsTotalValue(isActive);
        }

        public List<FullItemDetailDto> GetItemsWithGenresAndCategories()
        {
            return dbRepo.GetItemsWithGenresAndCategories();
        }

        public int UpsertItem(CreateOrUpdateItemDto item)
        {
            if (item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category id before insert or update");
            }

            return dbRepo.UpsertItem(mapper.Map<Item>(item));
        }

        public void UpsertItems(List<CreateOrUpdateItemDto> items)
        {
            try
            {
                dbRepo.UpsertItems(mapper.Map<List<Item>>(items));
            }
            catch (Exception ex)
            {
                // TODO: logging
                Console.WriteLine($"THe transaction has failed: {ex.Message}");
            }
        }

        public void DeleteItem(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid item id before deleting");
            }

            dbRepo.DeleteItem(id);
        }

        public void DeleteItems(List<int> itemIds)
        {
            try
            {
                dbRepo.DeteleItems(itemIds);
            }
            catch (Exception ex)
            {
                // TODO: logging
                Console.WriteLine($"THe transaction has failed: {ex.Message}");
            }
        }
    }
}
