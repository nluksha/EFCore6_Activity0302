﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models.DTOs
{
    public class FullItemDetailDto
    {
        public int Id { get; set; }
        public string ItemName { get; set; } = string.Empty;
        public string ItemDescription { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Notes { get; set; } = string.Empty;
        public decimal? CurrentOrFinalPrice { get; set; }
        public bool IsOnSale { get; set; }
        public DateTime? PurchasedDate { get; set; }
        public decimal? PurchasePrice { get; set; }
        public int? Quantity { get; set; }
        public DateTime? SoldDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool? CategoryIsActive { get; set; }
        public bool? CategoryIsDeleted { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public string ColorValue { get; set; } = string.Empty;
        public string PlayerName { get; set; } = string.Empty;
        public string PlayerDescription { get; set; } = string.Empty;
        public bool? PlayerIsActive { get; set; }
        public bool? PlayerIsDeleted { get; set; }
        public string GenreName { get; set; } = string.Empty;
        public bool? GenreIsActive { get; set; }
        public bool? GenreIsDeleted { get; set; }
    }
}
