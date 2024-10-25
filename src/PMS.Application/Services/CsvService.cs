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

        // Using this method to get the csv file
        public List<ProductWithoutIdDto> GetProductWithoutIdFromCsv(string filepath)
        {
            var csvData = _csvHandler.GetCsv(filepath);
            var products = new List<ProductWithoutIdDto>();

            // Starting from one, since first line is header
            for (int i = 1; i < csvData.Count; i++)
            {
                var line = csvData[i];
                // Semicolon as delimiter
                var fields = line.Split(';');
                // Create a ProductDto from each CSV line
                var product = new ProductWithoutIdDto
                {                    
                    Sku = fields[0],
                    Ean = fields[1],
                    Name = fields[2],
                    Description = fields[3],
                    Color = fields[4],
                    Material = fields[5],
                    ProductType = fields[6],
                    ProductGroup = fields[7],
                    Supplier = fields[8],
                    SupplierSku = fields[9],
                    TemplateNo = int.Parse(fields[10]),
                    List = int.Parse(fields[11]),
                    Weight = float.Parse(fields[12]),
                    Currency = fields[13],
                    Cost = float.Parse(fields[14]),                    
                    Price = float.Parse(fields[15]),
                    SpecialPrice = float.Parse(fields[16])
                };
                products.Add(product);
            }
            return products;
        }

        public async Task AddManyProductsFromCsv(string filepath){
            // Get the products from the CSV, and get the list of <ProductWithoutIdDto>
            var products = GetProductWithoutIdFromCsv(filepath);

            // Add the products with AddManyProducts from ProductService
            await _productService.AddManyProducts(products);            
        }    
    }
}
