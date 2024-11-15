using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PMS.Core.Entities;

namespace PMS.Infrastructure.Data
{
    public class PMSContext : DbContext
    {
        public PMSContext(DbContextOptions<PMSContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(ConfigureProduct);
            modelBuilder.Entity<Category>(ConfigureCategory);
            SeedData(modelBuilder);
        }

        private void ConfigureProduct(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasMany(p => p.Category).WithMany(c => c.Products)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryProduct",
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId"),
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId")
                );
            // Converting a list to a string
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => string.Join(',', v), 
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
            );            

            builder.Property(p => p.Material)
                .HasConversion(stringListConverter);

            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.ShopifyId).HasMaxLength(255);
            builder.Property(p => p.Sku).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Ean).HasMaxLength(255);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(255);
            builder.Property(p => p.Description).HasMaxLength(5000);
            builder.Property(p => p.Color).HasMaxLength(255);
            // builder.Property(p => p.Material).HasMaxLength(255);
            builder.Property(p => p.ProductType).HasMaxLength(255);
            builder.Property(p => p.ProductGroup).HasMaxLength(255);
            builder.Property(p => p.Supplier).HasMaxLength(255);
            builder.Property(p => p.SupplierSku).HasMaxLength(255);
            builder.Property(p => p.TemplateNo);
            builder.Property(p => p.List);
            builder.Property(p => p.Weight);
            builder.Property(p => p.Cost);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.SpecialPrice);
            builder.Property(p => p.Currency).HasMaxLength(10);
        }

        private void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasMany(c => c.Products).WithMany(p => p.Category)
                .UsingEntity<Dictionary<string, object>>(
                    "CategoryProduct",
                    j => j.HasOne<Product>().WithMany().HasForeignKey("ProductId"),
                    j => j.HasOne<Category>().WithMany().HasForeignKey("CategoryId")
                );
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(255);
            builder.Property(c => c.Description).HasMaxLength(5000);
            builder.Property(c => c.BottomDescription).HasMaxLength(65535);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic items", BottomDescription = "Various electronic items" },
                new Category { Id = 2, Name = "Books", Description = "Books of all genres", BottomDescription = "Various books" },
                new Category { Id = 3, Name = "iPhone 12", Description = "A description texts", BottomDescription = "A bottom description" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop" },
                new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone" },
                new Product { Id = 3, Name = "Novel", Description = "A best-selling novel" },
                new Product
                {
                    Id = 4,
                    Sku = "LC01-76-1038-1",
                    Ean = "EAN090909",
                    Name = "iPhone 12 / 12 Pro cover - Black",
                    Description = "A black iPhone 12 / 12 Pro cover",
                    Color = "Black",
                    // Material = "Silicone / TPU",
                    Material = new List<string> {"Silicone / TPU", "PU Leather"},
                    ProductType = "Cover",
                    ProductGroup = "Smartphone",
                    Supplier = "TVC",
                    SupplierSku = "101123911A",
                    TemplateNo = 11,
                    List = 447,
                    Weight = (float)0.026m,
                    Cost = (float)5.00m,
                    Price = (float)17.00m,
                    SpecialPrice = (float)12.95m,
                    Currency = "EUR"
                }
            );

            modelBuilder.Entity("CategoryProduct").HasData(
            new { CategoryId = 1, ProductId = 1 },
            new { CategoryId = 1, ProductId = 2 },
            new { CategoryId = 2, ProductId = 3 },
            new { CategoryId = 3, ProductId = 4 }
        );
        }
    }
}
