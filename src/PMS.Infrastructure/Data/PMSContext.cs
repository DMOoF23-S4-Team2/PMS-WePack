using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PMS.Core.Entities;

namespace PMS.Infrastructure.Data
{
    public class PMSContext : DbContext
    {
        public PMSContext(DbContextOptions<PMSContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(ConfigureProduct);
            modelBuilder.Entity<Category>(ConfigureCategory);

            //SeedData(modelBuilder);
        }

        //REVIEW - Vi burde tage en snak om hvilke properties der skal være IsRequired og hvilke der ikke skal.
        private void ConfigureProduct(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");
            builder.HasMany(p => p.Category).WithMany(c => c.Products);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Sku).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Ean).IsRequired().HasMaxLength(50);
            builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Description).IsRequired().HasMaxLength(500);
            builder.Property(p => p.Color).HasMaxLength(50);
            builder.Property(p => p.Material).HasMaxLength(50);
            builder.Property(p => p.ProductType).HasMaxLength(50);
            builder.Property(p => p.ProductGroup).HasMaxLength(50);
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.SpecialPrice);
        }

        private void ConfigureCategory(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");
            builder.HasMany(c => c.Products).WithMany(p => p.Category);
            builder.Property(c => c.Id).ValueGeneratedOnAdd();
            builder.Property(c => c.Name).IsRequired().HasMaxLength(100);
            builder.Property(c => c.Description).HasMaxLength(500);
            builder.Property(c => c.BottomDescription).HasMaxLength(500);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Electronics", Description = "Electronic items", BottomDescription = "Various electronic items" },
                new Category { Id = 2, Name = "Books", Description = "Books of all genres", BottomDescription = "Various books" }
            );

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Laptop", Description = "A high-performance laptop", Category = [new Category { Id = 1 }] },
                new Product { Id = 2, Name = "Smartphone", Description = "A latest model smartphone", Category = [new Category { Id = 1 }]},
                new Product { Id = 3, Name = "Novel", Description = "A best-selling novel", Category = [new Category { Id = 2 }]}
            );
        }
    }
}