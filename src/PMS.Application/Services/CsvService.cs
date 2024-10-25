using System;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;
using PMS.Infrastructure.Interfaces;


namespace PMS.Application.Services
{
    public class CsvService : ICsvService
    {
        private readonly ICsvHandler _csvHandler;
        private readonly IProductService _productService;

        public CsvService(ICsvHandler csvHandler, IProductService productService)
        {
            // Intialize CsvHandler from Infrastructure layer
            _csvHandler = csvHandler;
            _productService = productService;
        }
                
        public async Task CreateProduct(string filepath){ 
            // Get the products from the CSV, and get the list of <ProductWithoutIdDto>
            List<ProductWithoutIdDto> products = getProductFromCsv<ProductWithoutIdDto>(filepath);
            // Getting the first product
            ProductWithoutIdDto product = products[0];
            // Add the products with AddManyProducts from ProductService
            await _productService.CreateProduct(product);            
        }

        // public async Task DeleteProduct(string filepath){ 
        //     // Get the products from the CSV, and get the list of <ProductWithoutIdDto>
        //     List<ProductDto> products = getProductFromCsv<ProductDto>(filepath);
        //     // Getting the first product
        //     ProductDto productId = intproducts[0].Id;
        //     await _productService.DeleteProduct(productId);   
        // }

        // public async Task GetProduct(string filepath){ return; }
        // public async Task GetProducts(string filepath){ return; }
        // public async Task UpdateProduct(string filepath){ return; }        
        public async Task AddManyProductsFromCsv(string filepath){            
            List<ProductWithoutIdDto> products = getProductFromCsv<ProductWithoutIdDto>(filepath);            
            await _productService.AddManyProducts(products);            
        }

        // public async Task UpdateManyProducts(string filepath){ return; }
        // public async Task DeleteManyProducts(string filepath){ return; }

        // Methods to get .csv        
        private List<T> getProductFromCsv<T>(string filepath) where T : IProduct, new()
        {
            var csvData = _csvHandler.GetCsv(filepath);
            var products = new List<T>();

            // Parse header row to get column mappings
            var headers = csvData[0].Split(';');
            var headerIndexMap = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                headerIndexMap[headers[i].ToLower()] = i;
            }
            // Check if ID is in column
            bool hasIdColumn = headerIndexMap.ContainsKey("id");

            for (int i = 1; i < csvData.Count; i++)
            {
                var line = csvData[i];
                var fields = line.Split(';');

                // Create a new product object using the generic type, so we can decide type
                var product = new T();

                // Populate common properties
                product.Sku = fields[headerIndexMap["sku"]];
                product.Ean = fields[headerIndexMap["ean"]];
                product.Name = fields[headerIndexMap["name"]];
                product.Description = fields[headerIndexMap["description"]];
                product.Color = fields[headerIndexMap["color"]];
                product.Material = fields[headerIndexMap["material"]];
                product.ProductType = fields[headerIndexMap["product_type"]];
                product.ProductGroup = fields[headerIndexMap["product_group"]];
                product.Supplier = fields[headerIndexMap["supplier"]];
                product.SupplierSku = fields[headerIndexMap["supplier_sku"]];
                product.TemplateNo = int.Parse(fields[headerIndexMap["template_no"]]);
                product.List = int.Parse(fields[headerIndexMap["list"]]);
                product.Weight = float.Parse(fields[headerIndexMap["weight"]]);
                product.Currency = fields[headerIndexMap["currency"]];
                product.Cost = float.Parse(fields[headerIndexMap["cost"]]);
                product.Price = float.Parse(fields[headerIndexMap["price"]]);
                product.SpecialPrice = float.Parse(fields[headerIndexMap["special_price"]]);

                // Only set id if the column exists
                if (hasIdColumn && typeof(T) == typeof(ProductDto))
                {
                    var productWithId = product as ProductDto;
                    productWithId.Id = int.Parse(fields[headerIndexMap["id"]]);
                }

                products.Add(product);
            }
            return products;
        }        
       
    }
}
