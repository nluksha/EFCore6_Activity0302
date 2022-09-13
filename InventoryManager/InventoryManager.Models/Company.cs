using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models
{
    [Table("Companies")]
    public class Company: Player
    {
        [StringLength(InventoryModelsConstants.MAX_COMPANYNAME_LENGTH)] public string CompanyName { get; set; }
        [StringLength(InventoryModelsConstants.MAX_STOCKSYMBOL_LENGTH)] public string StockSymbol { get; set; }
        [StringLength(InventoryModelsConstants.MAX_CITY_LENGTH)] public string City { get; set; }

        public override string Name => $"{CompanyName} - {StockSymbol}";
    }
}
