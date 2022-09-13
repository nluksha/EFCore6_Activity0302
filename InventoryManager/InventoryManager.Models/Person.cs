using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models
{
    [Table("People")]
    public class Person: Player
    {
        [StringLength(InventoryModelsConstants.MAX_FIRSTNAME_LENGTH)] public string FirstName { get; set; }
        [StringLength(InventoryModelsConstants.MAX_LASTNAME_LENGTH)] public string LastName { get; set; }

        public override string Name => $"{FirstName} {LastName}";
    }
}
