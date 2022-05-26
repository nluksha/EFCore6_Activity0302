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
        List<Item> GetItems();
        List<ItemDto> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        List<GetItemsForListingDto> GetItemsForListingFromProcedure();
        List<GetItemsTotalValueDto> GetItemsTotalValue(bool isActive);
        List<FullItemDetailDto> GetItemsWithGenresAndCategories();

        int UpsertItem(Item item);
        void UpsertItems(List<Item> items);
        void DeleteItem(int id);
        void DeteleItems(List<int> itemIds);
    }
}
