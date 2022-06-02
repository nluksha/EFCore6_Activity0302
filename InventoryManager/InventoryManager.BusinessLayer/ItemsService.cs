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

        public ItemsService(IItemsRepo dbRepo, IMapper mapper)
        {
            this.mapper = mapper;
            this.dbRepo = dbRepo;
        }

        public async Task<string> GetAllItemsPipeDelimitedString()
        {
            var items = await GetItems();

            return String.Join('|', items);
        }

        public async Task<List<ItemDto>> GetItems()
        {
            return mapper.Map<List<ItemDto>>(await dbRepo.GetItems());
        }

        public async Task<List<ItemDto>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue)
        {
            return await dbRepo.GetItemsByDateRange(minDateValue, maxDateValue);
        }

        public async Task<List<GetItemsForListingDto>> GetItemsForListingFromProcedure()
        {
            return await dbRepo.GetItemsForListingFromProcedure();
        }

        public async Task<List<GetItemsTotalValueDto>> GetItemsTotalValue(bool isActive)
        {
            return await dbRepo.GetItemsTotalValue(isActive);
        }

        public async Task<List<FullItemDetailDto>> GetItemsWithGenresAndCategories()
        {
            return await dbRepo.GetItemsWithGenresAndCategories();
        }

        public async Task<int> UpsertItem(CreateOrUpdateItemDto item)
        {
            if (item.CategoryId <= 0)
            {
                throw new ArgumentException("Please set the category id before insert or update");
            }

            return await dbRepo.UpsertItem(mapper.Map<Item>(item));
        }

        public async Task UpsertItems(List<CreateOrUpdateItemDto> items)
        {
            try
            {
                await dbRepo.UpsertItems(mapper.Map<List<Item>>(items));
            }
            catch (Exception ex)
            {
                // TODO: logging
                Console.WriteLine($"THe transaction has failed: {ex.Message}");
            }
        }

        public async Task DeleteItem(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Please set a valid item id before deleting");
            }

            await dbRepo.DeleteItem(id);
        }

        public async Task DeleteItems(List<int> itemIds)
        {
            try
            {
                await dbRepo.DeteleItems(itemIds);
            }
            catch (Exception ex)
            {
                // TODO: logging
                Console.WriteLine($"THe transaction has failed: {ex.Message}");
            }
        }
    }
}
