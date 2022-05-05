using InventoryManager.DbLibrary;
using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Migrator
{
    public class BuildCategories
    {
        private readonly InventoryDbContext context;
        private const string SEED_USER_ID = "9164f960-7946-487a-aa77-c46e9a403568";

        public BuildCategories(InventoryDbContext context)
        {
            this.context = context;
        }

        public void ExecuteSeed()
        {
            if (context.Categories.Count() == 0)
            {
                context.Categories.AddRange(
                    new Category
                    {
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        IsDeleted = false,
                        Name = "Movies",
                        CategoryDetail = new CategoryDetail
                        {
                            ColorValue = "@0000FF",
                            ColorName = "Blue"
                        }
                    },
                   new Category
                   {
                       CreatedDate = DateTime.Now,
                       IsActive = true,
                       IsDeleted = false,
                       Name = "Books",
                       CategoryDetail = new CategoryDetail
                       {
                           ColorValue = "@FF0000",
                           ColorName = "Red"
                       }
                   },
                   new Category
                   {
                       CreatedDate = DateTime.Now,
                       IsActive = true,
                       IsDeleted = false,
                       Name = "Games",
                       CategoryDetail = new CategoryDetail
                       {
                           ColorValue = "@008000",
                           ColorName = "Green"
                       }
                   });

                context.SaveChanges();
            }
        }
    }
}
