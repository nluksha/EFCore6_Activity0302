using InventoryManager.Models.DTOs;

namespace InventoryManager.BusinessLayer
{
    public interface IItemsService
    {
        List<ItemDto> GetItems();
        List<ItemDto> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        List<GetItemsForListingDto> GetItemsForListingFromProcedure();
        List<GetItemsTotalValueDto> GetItemsTotalValue(bool isActive);
        string GetAllItemsPipeDelimitedString();
        List<FullItemDetailDto> GetItemsWithGenresAndCategories();

        int UpsertItem(CreateOrUpdateItemDto item);
        void UpsertItems(List<CreateOrUpdateItemDto> items);
        void DeleteItem(int id);
        void DeleteItems(List<int> itemIds);
    }
}