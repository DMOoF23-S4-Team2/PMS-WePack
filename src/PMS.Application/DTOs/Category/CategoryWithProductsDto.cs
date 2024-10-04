using PMS.Application.DTOs.Product;

namespace PMS.Application.DTOs.Category;

//REVIEW - remove if CategoryWithProductsDto is not needed
public class CategoryWithProductsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string BottomDescription { get; set; } = string.Empty;
    public ICollection<ProductDto> Products { get; set; } = [];
}