using System;

namespace PMS.Application.DTO_s.Category;

public class CategoryDto
{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string BottomDescription { get; set; } = string.Empty;

        // public ICollection<Product> Products { get; set; } = [];

}
