using PMS.Application.DTOs.Product;

namespace PMS.Application.DTOs.Category;
public class CategoryDto
{
        public int Id { get; set; }        
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BottomDescription { get; set; } = string.Empty;
}
