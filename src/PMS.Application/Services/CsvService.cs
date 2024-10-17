using System;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;
using PMS.Infrastructure.FileHandlers;

namespace PMS.Application.Services
{
    public class CsvService : ICsvService
    {
        private readonly CsvHandler _csvHandler;

        public CsvService()
        {
            // Intialize CsvHandler from Infrastructure layer
            _csvHandler = new CsvHandler();            
        }

        public List<ProductDto> GetProductCsv(string filepath)
        {
            var csvData = _csvHandler.GetCsv(filepath);
            var products = new List<ProductDto>();

            // Starting from one, since first line is header
            for (int i = 1; i < csvData.Count; i++)
            {
                var line = csvData[i];
                // Semicolon as delimiter
                var fields = line.Split(';');
                // Create a ProductDto from each CSV line
                var product = new ProductDto
                {
                    Id = int.Parse(fields[0]),
                    Sku = fields[1],
                    Ean = fields[2],
                    Name = fields[3],
                    Description = fields[4],
                    Color = fields[5],
                    Material = fields[6],
                    ProductType = fields[7],
                    ProductGroup = fields[8],
                    Supplier = fields[9],
                    SupplierSku = fields[10],
                    TemplateNo = int.Parse(fields[11]),
                    List = int.Parse(fields[12]),
                    Weight = float.Parse(fields[13]),
                    Currency = fields[14],
                    Cost = float.Parse(fields[15]),                    
                    Price = float.Parse(fields[16]),
                    SpecialPrice = float.Parse(fields[17])
                };
                products.Add(product);
            }
            return products;
        }
    }
}
