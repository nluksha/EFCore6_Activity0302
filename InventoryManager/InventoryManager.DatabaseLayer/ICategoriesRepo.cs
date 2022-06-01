using InventoryManager.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.DatabaseLayer
{
    public interface ICategoriesRepo
    {
        Task<List<CategoryDto>> ListCategoriesAndDetails();
    }
}
