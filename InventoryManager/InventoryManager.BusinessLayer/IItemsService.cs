using InventoryManager.Models.DTOs;

namespace InventoryManager.BusinessLayer
{
    public interface IItemsService
    {
        Task<List<ItemDto>> GetItems();
        Task<List<ItemDto>> GetItemsByDateRange(DateTime minDateValue, DateTime maxDateValue);
        Task<List<GetItemsForListingDto>> GetItemsForListingFromProcedure();
        Task<List<GetItemsTotalValueDto>> GetItemsTotalValue(bool isActive);
        Task<string> GetAllItemsPipeDelimitedString();
        Task<List<FullItemDetailDto>> GetItemsWithGenresAndCategories();

        Task<int> UpsertItem(CreateOrUpdateItemDto item);
        Task UpsertItems(List<CreateOrUpdateItemDto> items);
        Task DeleteItem(int id);
        Task DeleteItems(List<int> itemIds);
    }
}