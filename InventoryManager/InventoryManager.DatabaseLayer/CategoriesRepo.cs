using AutoMapper;
using AutoMapper.QueryableExtensions;
using InventoryManager.DbLibrary;
using InventoryManager.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.DatabaseLayer
{
    public class CategoriesRepo: ICategoriesRepo
    {
        private readonly IMapper mapper;
        private readonly InventoryDbContext context;

        public CategoriesRepo(IMapper mapper, InventoryDbContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public List<CategoryDto> ListCategoriesAndDetails()
        {
            return context.Categories
                .Include(x => x.CategoryDetail)
                .ProjectTo<CategoryDto>(mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
