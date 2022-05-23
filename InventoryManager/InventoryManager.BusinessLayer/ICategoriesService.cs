using InventoryManager.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.BusinessLayer
{
    public interface ICategoriesService
    {
        List<CategoryDto> ListCategoriesAndDetails();
    }
}
