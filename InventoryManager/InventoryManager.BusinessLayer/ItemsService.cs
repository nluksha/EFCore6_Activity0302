using AutoMapper;
using InventoryManager.DatabaseLayer;
using InventoryManager.DbLibrary;
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

        public ItemsService(InventoryDbContext context, IMapper mapper)
        {
            dbRepo = new ItemsRepo(context, mapper);
        }

        public string GetAllItemsPipeDelimitedString()
        {
            var items = GetItems();

            return String.Join('|', items);
        }

        public List<ItemDto> GetItems()
        {
            return dbRepo.GetItems();
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
    }
}
