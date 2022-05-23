using AutoMapper;
using InventoryManager.DatabaseLayer;
using InventoryManager.DbLibrary;
using InventoryManager.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.BusinessLayer
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ICategoriesRepo dbRepo;

        public CategoriesService(InventoryDbContext context, IMapper mapper)
        {
            dbRepo = new CategoriesRepo(context, mapper);
        }

        public List<CategoryDto> ListCategoriesAndDetails()
        {
            return dbRepo.ListCategoriesAndDetails();
        }
    }
}
