using System;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;
using PMS.Infrastructure.Interfaces;
// using PMS.Infrastructure.FileHandlers;

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
                
        // public Task CreateProduct(string filepath){ return; }
        // public Task DeleteProduct(string filepath){ return; }
        // public Task GetProduct(string filepath){ return; }
        // public Task GetProducts(string filepath){ return; }
        // public Task UpdateProduct(string filepath){ return; }        
        public async Task AddManyProductsFromCsv(string filepath){
            // Get the products from the CSV, and get the list of <ProductWithoutIdDto>
            var products = GetProductWithoutIdFromCsv(filepath);
            // Add the products with AddManyProducts from ProductService
            await _productService.AddManyProducts(products);            
        }

        // public Task UpdateManyProducts(string filepath){ return; }
        // public Task DeleteManyProducts(string filepath){ return; }

        // Methods to get .csv
        public List<ProductWithoutIdDto> GetProductWithoutIdFromCsv(string filepath)
        {
            // Parse the csv
            var csvData = _csvHandler.GetCsv(filepath);
            var products = new List<ProductWithoutIdDto>();            
            // First row is headers            
            var headers = csvData[0].Split(';');
            // Map header columns into names / not by index. Key: column name. value: Index
            var headerIndexMap = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                headerIndexMap[headers[i].ToLower()] = i;
            }
            // Loop through products
            for (int i = 1; i < csvData.Count; i++){
                // Each line
                var line = csvData[i];
                // Split the line                
                var fields = line.Split(';');
                var product = new ProductWithoutIdDto
                {
                    Sku = fields[headerIndexMap["sku"]],
                    Ean = fields[headerIndexMap["ean"]],
                    Name = fields[headerIndexMap["name"]],
                    Description = fields[headerIndexMap["description"]],
                    Color = fields[headerIndexMap["color"]],
                    Material = fields[headerIndexMap["material"]],
                    ProductType = fields[headerIndexMap["product_type"]],
                    ProductGroup = fields[headerIndexMap["product_group"]],
                    Supplier = fields[headerIndexMap["supplier"]],
                    SupplierSku = fields[headerIndexMap["supplier_sku"]],
                    TemplateNo = int.Parse(fields[headerIndexMap["template_no"]]),
                    List = int.Parse(fields[headerIndexMap["list"]]),
                    Weight = float.Parse(fields[headerIndexMap["weight"]]),
                    Currency = fields[headerIndexMap["currency"]],
                    Cost = float.Parse(fields[headerIndexMap["cost"]]),
                    Price = float.Parse(fields[headerIndexMap["price"]]),
                    SpecialPrice = float.Parse(fields[headerIndexMap["special_price"]])
                };
                // Add to list
                products.Add(product);                        
            }

            // Starting from one, since first line is header
            // for (int i = 1; i < csvData.Count; i++)
            // {
            //     var line = csvData[i];
            //     // Semicolon as delimiter
            //     var fields = line.Split(';');
            //     // Create a ProductDto from each CSV line
            //     var product = new ProductWithoutIdDto
            //     {                    
            //         Sku = fields[0],
            //         Ean = fields[1],
            //         Name = fields[2],
            //         Description = fields[3],
            //         Color = fields[4],
            //         Material = fields[5],
            //         ProductType = fields[6],
            //         ProductGroup = fields[7],
            //         Supplier = fields[8],
            //         SupplierSku = fields[9],
            //         TemplateNo = int.Parse(fields[10]),
            //         List = int.Parse(fields[11]),
            //         Weight = float.Parse(fields[12]),
            //         Currency = fields[13],
            //         Cost = float.Parse(fields[14]),                    
            //         Price = float.Parse(fields[15]),
            //         SpecialPrice = float.Parse(fields[16])
            //     };
            //     products.Add(product);
            // }
            foreach(var p in products){
                System.Console.WriteLine(p.Name);
            }

            return products;
        }
    
    
    }
}
