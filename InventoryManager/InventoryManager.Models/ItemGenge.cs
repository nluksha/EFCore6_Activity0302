using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryManager.Models.Interfaces;

namespace InventoryManager.Models
{
    [Table("ItemGenges")]
    public class ItemGenge: IIdentityModel
    {
        public int Id { get; set; }

        public virtual int ItemId { get; set; }
        public virtual Item Item { get; set; }

        public virtual int GenreId { get; set; }
        public virtual Genre Genre { get; set; }
    }
}
