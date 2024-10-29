﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PMS.Infrastructure.Data;

#nullable disable

namespace PMS.Infrastructure.Migrations
{
    [DbContext(typeof(PMSContext))]
    partial class PMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.HasKey("CategoryId", "ProductId");

                    b.HasIndex("ProductId");

                    b.ToTable("CategoryProduct");

                    b.HasData(
                        new
                        {
                            CategoryId = 1,
                            ProductId = 1
                        },
                        new
                        {
                            CategoryId = 1,
                            ProductId = 2
                        },
                        new
                        {
                            CategoryId = 2,
                            ProductId = 3
                        },
                        new
                        {
                            CategoryId = 3,
                            ProductId = 4
                        });
                });

            modelBuilder.Entity("PMS.Core.Entities.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("BottomDescription")
                        .IsRequired()
                        .HasMaxLength(65535)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Categories", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BottomDescription = "Various electronic items",
                            Description = "Electronic items",
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 2,
                            BottomDescription = "Various books",
                            Description = "Books of all genres",
                            Name = "Books"
                        },
                        new
                        {
                            Id = 3,
                            BottomDescription = "A bottom description",
                            Description = "A description texts",
                            Name = "iPhone 12"
                        });
                });

            modelBuilder.Entity("PMS.Core.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<float>("Cost")
                        .HasColumnType("real");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ean")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("List")
                        .HasColumnType("int");

                    b.Property<string>("Material")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<float>("Price")
                        .HasColumnType("real");

                    b.Property<string>("ProductGroup")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ProductType")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ShopifyId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<float>("SpecialPrice")
                        .HasColumnType("real");

                    b.Property<string>("Supplier")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("SupplierSku")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("TemplateNo")
                        .HasColumnType("int");

                    b.Property<float>("Weight")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Color = "",
                            Cost = 0f,
                            Currency = "",
                            Description = "A high-performance laptop",
                            Ean = "",
                            List = 0,
                            Material = "",
                            Name = "Laptop",
                            Price = 0f,
                            ProductGroup = "",
                            ProductType = "",
                            ShopifyId = "",
                            Sku = "",
                            SpecialPrice = 0f,
                            Supplier = "",
                            SupplierSku = "",
                            TemplateNo = 0,
                            Weight = 0f
                        },
                        new
                        {
                            Id = 2,
                            Color = "",
                            Cost = 0f,
                            Currency = "",
                            Description = "A latest model smartphone",
                            Ean = "",
                            List = 0,
                            Material = "",
                            Name = "Smartphone",
                            Price = 0f,
                            ProductGroup = "",
                            ProductType = "",
                            ShopifyId = "",
                            Sku = "",
                            SpecialPrice = 0f,
                            Supplier = "",
                            SupplierSku = "",
                            TemplateNo = 0,
                            Weight = 0f
                        },
                        new
                        {
                            Id = 3,
                            Color = "",
                            Cost = 0f,
                            Currency = "",
                            Description = "A best-selling novel",
                            Ean = "",
                            List = 0,
                            Material = "",
                            Name = "Novel",
                            Price = 0f,
                            ProductGroup = "",
                            ProductType = "",
                            ShopifyId = "",
                            Sku = "",
                            SpecialPrice = 0f,
                            Supplier = "",
                            SupplierSku = "",
                            TemplateNo = 0,
                            Weight = 0f
                        },
                        new
                        {
                            Id = 4,
                            Color = "Black",
                            Cost = 5f,
                            Currency = "EUR",
                            Description = "A black iPhone 12 / 12 Pro cover",
                            Ean = "EAN090909",
                            List = 447,
                            Material = "Silicone / TPU",
                            Name = "iPhone 12 / 12 Pro cover - Black",
                            Price = 17f,
                            ProductGroup = "Smartphone",
                            ProductType = "Cover",
                            ShopifyId = "",
                            Sku = "LC01-76-1038-1",
                            SpecialPrice = 12.95f,
                            Supplier = "TVC",
                            SupplierSku = "101123911A",
                            TemplateNo = 11,
                            Weight = 0.026f
                        });
                });

            modelBuilder.Entity("CategoryProduct", b =>
                {
                    b.HasOne("PMS.Core.Entities.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PMS.Core.Entities.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
