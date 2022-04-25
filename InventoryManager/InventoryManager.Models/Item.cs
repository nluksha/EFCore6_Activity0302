﻿using System.ComponentModel.DataAnnotations;

namespace InventoryManager.Models
{
    public class Item: FullAuditModel
    {
        [StringLength(InventoryModelsConstans.MAX_NAME_LENGTH)]
        public string? Name { get; set; }

        public int Quantity { get; set; }

        [StringLength(InventoryModelsConstans.MAX_DESCRIPTION_LENGTH)]
        public string? Description { get; set; }

        [StringLength(InventoryModelsConstans.MAX_NOTES_LENGTH, MinimumLength = InventoryModelsConstans.MIN_NOTES_LENGTH)]
        public string? Notes { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? PurchasedDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? CurrentOrFinalPrice { get; set; }
    }
}