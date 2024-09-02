namespace PMS.Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Ean { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color {get; set;} = string.Empty;
        public string Material {get; set;} = string.Empty;
        public string ProductType {get; set;} = string.Empty;
        public string ProductGroup {get; set;} = string.Empty;
        public string Currency {get; set;} = string.Empty;
        public float Price {get; set;}
        public float SpecialPrice {get; set; }
        public ICollection<Category> Category { get; set; } = [];
    }
}