using System;
using System.Threading.Tasks;
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
            List<ProductWithoutIdDto> products = getProductWithoutIDFromCsv(filepath);
            // Take the first product
            await _productService.CreateProduct(products[0]);            
        }

        public async Task DeleteProduct(string filepath){ 
            // Get the products from the CSV, and get the list of <ProductWithoutIdDto>
            List<ProductDto> products = getProductWithIDFromCsv(filepath);
            // Getting the first product
            int productId = products[0].Id;
            await _productService.DeleteProduct(productId);   
        }

        public async Task GetProduct(string filepath){             
            List<ProductDto> products = getProductWithIDFromCsv(filepath);
            // Getting the first product
            int productId = products[0].Id;
            await _productService.GetProduct(productId); 
        }

        public async Task GetProducts(string filepath){ 
            List<ProductDto> products = getProductWithIDFromCsv(filepath);
            await _productService.GetProducts(); 
        }

        public async Task AddManyProductsFromCsv(string filepath){                        
            List<ProductWithoutIdDto> products = getProductWithoutIDFromCsv(filepath);            
            await _productService.AddManyProducts(products);            
        }

        public async Task DeleteManyProducts(string filepath){ 
            List<ProductDto> products = getProductWithIDFromCsv(filepath);
            await _productService.DeleteManyProducts(products);             
        }
        // public async Task UpdateManyProducts(string filepath){ return; }

        // public async Task UpdateProduct(string filepath){ 
        //     List<ProductDto> products = getProductWithIDFromCsv(filepath);            
        //     int productId = products[0].Id;
        //     ProductDto product = products[0];
        //     await _productService.UpdateProduct(productId, product); 
        // }        

        // Methods to get .csv        
        private List<ProductDto> getProductWithIDFromCsv(string filepath)
        {
            var csvData = _csvHandler.GetCsv(filepath);
            var products = new List<ProductDto>();

            // Parse header row to get column mappings
            var headers = csvData[0].Split(';');
            var headerIndexMap = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                headerIndexMap[headers[i].ToLower()] = i;
            }

            for (int i = 1; i < csvData.Count; i++)
            {
                var line = csvData[i];
                var fields = line.Split(';');
                
                var product = new ProductDto(){                            
                    Id = int.Parse(fields[headerIndexMap["id"]]),
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
            return products;
        }        
       
        private List<ProductWithoutIdDto> getProductWithoutIDFromCsv(string filepath)
        {
            var csvData = _csvHandler.GetCsv(filepath);
            var products = new List<ProductWithoutIdDto>();

            // Parse header row to get column mappings
            var headers = csvData[0].Split(';');
            var headerIndexMap = new Dictionary<string, int>();
            for (int i = 0; i < headers.Length; i++)
            {
                headerIndexMap[headers[i].ToLower()] = i;
            }

            for (int i = 1; i < csvData.Count; i++)
            {
                var line = csvData[i];
                var fields = line.Split(';');
                
                var product = new ProductWithoutIdDto(){                                                
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
            return products;
        }  
    
    
    }
}
