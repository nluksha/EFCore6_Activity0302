﻿using InventoryManager.DbLibrary;
using InventoryManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManager.Migrator
{
    public class BuildItems
    {
        private readonly InventoryDbContext context;
        private const string SEED_USER_ID = "9164f960-7946-487a-aa77-c46e9a403568";

        public BuildItems(InventoryDbContext context)
        {
            this.context = context;
        }

        public void ExecuteSeed()
        {
            // DELETE FROM ItemPlayers; DELETE From Items; DELETE From Players
            if (context.Items.Count() == 0)
            {
                context.Items.AddRange(new Item()
                {
                    Name = "Batman Begins",
                    CurrentOrFinalPrice = 9.99m,
                    Description = "You either die the hero or live long enough to see yourself become the villain",
                    IsOnSale = false,
                    Notes = "",
                    PurchasePrice = 23.99m,
                    PurchasedDate = null,
                    Quantity = 1000,
                    SoldDate = null,
                    CreatedByUserId = SEED_USER_ID,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Players = new List<Player>() { new Player() {
                        CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, CreatedByUserId = SEED_USER_ID,
                        Description = "https://www.imdb.com/name/nm0000288/", Name = "Christian Bale"
                        }
                    }
                }, new Item()
                {
                    Name = "Inception",
                    CurrentOrFinalPrice = 7.99m,
                    Description = "You mustn't be afraid to dream a little bigger, darling",
                    IsOnSale = false,
                    Notes = "",
                    PurchasePrice = 4.99m,
                    PurchasedDate = null,
                    Quantity = 1000,
                    SoldDate = null,
                    CreatedByUserId = SEED_USER_ID,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Players = new List<Player>() { new Player() {
                        CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, CreatedByUserId = SEED_USER_ID,
                        Description = "https://www.imdb.com/name/nm0000138/", Name = "Leonardo DiCaprio" } }
                }, new Item()
                {
                    Name = "Remember the Titans",
                    CurrentOrFinalPrice = 3.99m,
                    Description = "Left Side, Strong Side",
                    IsOnSale = false,
                    Notes = "",
                    PurchasePrice = 7.99m,
                    PurchasedDate = null,
                    Quantity = 1000,
                    SoldDate = null,
                    CreatedByUserId = SEED_USER_ID,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Players = new List<Player>() { new Player() {
                        CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false, CreatedByUserId = SEED_USER_ID,
                        Description = "https://www.imdb.com/name/nm0000243/", Name = "Denzel Washington" } }
                }, new Item()
                {
                    Name = "Star Wars: The Empire Strikes Back",
                    CurrentOrFinalPrice = 19.99m,
                    Description = "He will join us or die, master",
                    IsOnSale = false,
                    Notes = "",
                    PurchasePrice = 35.99m,
                    PurchasedDate = null,
                    Quantity = 1000,
                    SoldDate = null,
                    CreatedByUserId = SEED_USER_ID,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Players = new List<Player>() { new Player() { CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false,
                        CreatedByUserId = SEED_USER_ID, Description = "https://www.imdb.com/name/nm0000434/", Name = "Mark Hamill" } }
                }, new Item()
                {
                    Name = "Top Gun",
                    CurrentOrFinalPrice = 6.99m,
                    Description = "I feel the need, the need for speed!",
                    IsOnSale = false,
                    Notes = "",
                    PurchasePrice = 8.99m,
                    PurchasedDate = null,
                    Quantity = 1000,
                    SoldDate = null,
                    CreatedByUserId = SEED_USER_ID,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    IsActive = true,
                    Players = new List<Player>() { new Player() {
                        CreatedDate = DateTime.Now, IsActive = true, IsDeleted = false,
                        CreatedByUserId = SEED_USER_ID, Description = "https://www.imdb.com/name/nm0000129/", Name = "Tom Cruise" } }
                });

                context.SaveChanges();
            }
        }
    }
}

