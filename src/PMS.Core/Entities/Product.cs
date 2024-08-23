public class Product
{
    public int Id { get; set; }
    public string Sku { get; set; }
    public string Ean { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Category> Category { get; set; }
    public string Color {get; set;}
    public string Material {get; set;}
    public string ProductType {get; set;}
    public string ProductGroup {get; set;}
    public float Price {get; set;}
    public float SpecialPrice {get; set; }
}