using System.ComponentModel.DataAnnotations;

namespace PMS.Core.Entities
{
    public class Product
    {        
        public int Id { get; set; }        
        
        [Required]
        [MaxLength(255)]
        public string Sku { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string Ean { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(255)]
        public string Name { get; set; } = string.Empty;
        
        [MaxLength(5000)]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string Color {get; set;} = string.Empty;
        
        [MaxLength(255)]
        public string Material {get; set;} = string.Empty;
        
        [MaxLength(255)]
        public string ProductType {get; set;} = string.Empty;
        
        [MaxLength(255)]
        public string ProductGroup {get; set;} = string.Empty;
        
        [MaxLength(255)]
        public string Supplier { get; set; } = string.Empty;
        
        [MaxLength(255)]
        public string SupplierSku { get; set; } = string.Empty;
                
        public int TemplateNo { get; set; }
                
        public int List { get; set; }
                
        public float Weight { get; set; }
                
        public float Cost { get; set; }

        [MaxLength(10)]
        public string Currency {get; set;} = string.Empty;
        
        [Required]
        [Range(0, float.MaxValue, ErrorMessage = "Price must be a positive number")]
        public float Price {get; set;}
        
        [Range(0, float.MaxValue, ErrorMessage = "Price must be a positive number")]
        public float SpecialPrice {get; set; }
        public ICollection<Category> Category { get; set; } = [];
    }
}