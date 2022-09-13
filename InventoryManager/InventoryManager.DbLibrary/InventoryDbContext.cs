using InventoryManager.Models;
using InventoryManager.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InventoryManager.DbLibrary
{
    public class InventoryDbContext : DbContext
    {
        private static IConfigurationRoot configuration;
        private const string systenUserId = "9164f960-7946-487a-aa77-c46e9a403568";

        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDetail> CategoryDetails { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Person> People { get; set; }

        // Stored Procedures
        public DbSet<GetItemsForListingDto> ItemsForLisitng { get; set; }

        // Functions
        public DbSet<AllItemsPipeDeliminatedStingDto> AllItemsOutput { get; set; }
        public DbSet<GetItemsTotalValueDto> GetItemsTotalValues { get; set; }

        // View
        public DbSet<FullItemDetailDto> FullItemDetailDtos { get; set; }

        public InventoryDbContext()
        {
        }

        public InventoryDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("InventoryManager");

                optionsBuilder.UseSqlServer(connectionString);
            }

            optionsBuilder.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>()
                .HasMany(x => x.Players)
                .WithMany(p => p.Items)
                .UsingEntity<Dictionary<string, object>>(
                "ItemPlayers",
                ip => ip.HasOne<Player>()
                    .WithMany()
                    .HasForeignKey("PlayerId")
                    .HasConstraintName("FK_ItemPlayers_Players_PlayerId")
                    .OnDelete(DeleteBehavior.Cascade),
                ip => ip.HasOne<Item>()
                    .WithMany()
                    .HasForeignKey("ItemId")
                    .HasConstraintName("FK_ItemPlayers_Items_ItemId")
                    .OnDelete(DeleteBehavior.Cascade)
                );

            // Stored Procedures
            modelBuilder.Entity<GetItemsForListingDto>(x =>
            {
                x.HasNoKey();
                x.ToView("ItemsForLisitng");
            });

            // Functions
            modelBuilder.Entity<AllItemsPipeDeliminatedStingDto>(x =>
            {
                x.HasNoKey();
                x.ToView("AllItemsOutput");
            });
            modelBuilder.Entity<GetItemsTotalValueDto>(x =>
            {
                x.HasNoKey();
                x.ToView("GetItemsTotalValues");
            });

            //View
            modelBuilder.Entity<FullItemDetailDto>(x =>
            {
                x.HasNoKey();
                x.ToView("FullItemDetailDtos");
            });

            // Seed Data
            var genreCreateDate = new DateTime(2021, 01, 01);
            modelBuilder.Entity<Genre>(x =>
            {
               x.HasData(
                   new Genre { Id = 1, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Fantasy" },
                   new Genre { Id = 2, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Sci/Fi" },
                   new Genre { Id = 3, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Horror" },
                   new Genre { Id = 4, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Comedy" },
                   new Genre { Id = 5, CreatedDate = genreCreateDate, IsActive = true, IsDeleted = false, Name = "Drama" }
               );
            });
        }

        public override int SaveChanges()
        {
            var tracker = ChangeTracker;

            foreach (var entry in tracker.Entries())
            {

                if (entry.Entity is FullAuditModel referenceEntity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            referenceEntity.CreatedDate = DateTime.Now;
                            if (string.IsNullOrWhiteSpace(referenceEntity.CreatedByUserId))
                            {
                                referenceEntity.CreatedByUserId = systenUserId;
                            }
                            break;
                        case EntityState.Modified:
                        case EntityState.Deleted:
                            referenceEntity.LastModifiedDate = DateTime.Now;
                            if (string.IsNullOrWhiteSpace(referenceEntity.LastModifiedUserId))
                            {
                                referenceEntity.LastModifiedUserId = systenUserId;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return base.SaveChanges();
        }

    }
}