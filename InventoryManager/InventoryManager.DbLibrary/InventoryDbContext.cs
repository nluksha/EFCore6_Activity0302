using InventoryManager.Models;
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