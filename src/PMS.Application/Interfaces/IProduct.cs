public interface IProduct
{
    string Sku { get; set; }
    string Ean { get; set; }
    string Name { get; set; }
    string Description { get; set; }
    string Color {get; set;}
    string Material {get; set;}
    string ProductType {get; set;}
    string ProductGroup {get; set;}       
    string Supplier { get; set; }
    string SupplierSku { get; set; }
    int TemplateNo { get; set; }
    int List { get; set; }
    float Weight { get; set; }
    float Cost { get; set; }
    string Currency {get; set;}
    float Price {get; set;}
    float SpecialPrice {get; set; }
}