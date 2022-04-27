using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models
{
    [Table("Players")]
    public class Player: FullAuditModel
    {
        [Required]
        [StringLength(InventoryModelsConstans.MAX_PLAYERNAME_LENGTH)]
        public string Name { get; set; }

        [StringLength(InventoryModelsConstans.MAX_PLAYERDESCRIPTION_LENGTH)]
        public string? Description { get; set; }

        public virtual List<Item> Items { get; set; } = new List<Item>();
    }
}
