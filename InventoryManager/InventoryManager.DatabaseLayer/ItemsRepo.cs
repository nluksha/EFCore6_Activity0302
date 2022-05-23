using AutoMapper;
using InventoryManager.Models.DTOs;
using InventoryManager.DbLibrary;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

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

        public List<ItemDto> GetItems()
        {
            var items = context.Items
                .ProjectTo<ItemDto>(mapper.ConfigurationProvider)
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
    }
}