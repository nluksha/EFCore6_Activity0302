using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Models.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Category { get; set; } = string.Empty;
        public CategoryDetailDto CategoryDetail { get; set; }
    }
}
