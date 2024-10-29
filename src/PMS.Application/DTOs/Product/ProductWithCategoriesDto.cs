using PMS.Application.DTOs.Category;

namespace PMS.Application.DTOs.Product;

//REVIEW - remove if ProductWithCategoriesDto is not needed
public class ProductWithCategoriesDto
{
    public int Id { get; set; }
    public string ShopifyId { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public string Ean { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Material { get; set; } = string.Empty;
    public string ProductType { get; set; } = string.Empty;
    public string ProductGroup { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string SupplierSku { get; set; } = string.Empty;
    public int TemplateNo { get; set; }
    public int List { get; set; }
    public float Weight { get; set; }
    public float Cost { get; set; }
    public string Currency { get; set; } = string.Empty;
    public float Price { get; set; }
    public float SpecialPrice { get; set; }
    public ICollection<CategoryDto> Category { get; set; } = [];
}