using System;
using System.ComponentModel.DataAnnotations;
using PMS.Application.DTOs.Product;

namespace PMS.Application.DTOs.Category;

public class CategoryDto
{
        public int Id { get; set; }        
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(65535)]
        public string BottomDescription { get; set; } = string.Empty;

        public ICollection<ProductDto> Products { get; set; } = [];

}