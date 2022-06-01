using InventoryManager.Models;
using InventoryManager.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.DatabaseLayer
{
    public interface IItemsRepo
    {
        Task<List<Item>> GetItems();
        Task<List<ItemDto>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        Task<List<GetItemsForListingDto>> GetItemsForListingFromProcedure();
        Task<List<GetItemsTotalValueDto>> GetItemsTotalValue(bool isActive);
        Task<List<FullItemDetailDto>> GetItemsWithGenresAndCategories();

        Task<int> UpsertItem(Item item);
        Task UpsertItems(List<Item> items);
        Task DeleteItem(int id);
        Task DeteleItems(List<int> itemIds);
    }
}
