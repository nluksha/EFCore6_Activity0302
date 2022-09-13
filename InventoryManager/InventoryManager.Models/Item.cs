using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryManager.Models
{
    public class Item: FullAuditModel
    {
        [Required]
        [Column(TypeName = "VARCHAR")]
        [StringLength(InventoryModelsConstants.MAX_NAME_LENGTH)]
        public string Name { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_QAUNTITY, InventoryModelsConstants.MAXIMUM_QAUNTITY)]
        public int Quantity { get; set; }

        [StringLength(InventoryModelsConstants.MAX_DESCRIPTION_LENGTH)]
        public string? Description { get; set; }

        [StringLength(InventoryModelsConstants.MAX_NOTES_LENGTH, MinimumLength = InventoryModelsConstants.MIN_NOTES_LENGTH)]
        public string? Notes { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? PurchasedDate { get; set; }
        public DateTime? SoldDate { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_PRICE, InventoryModelsConstants.MAXIMUM_PRICE)]
        public decimal? PurchasePrice { get; set; }

        [Range(InventoryModelsConstants.MINIMUM_PRICE, InventoryModelsConstants.MAXIMUM_PRICE)]
        public decimal? CurrentOrFinalPrice { get; set; }

        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual List<Player> Players { get; set; } = new List<Player>();

        public virtual List<ItemGenge> ItemGenres { get; set; } = new List<ItemGenge>();
    }
}