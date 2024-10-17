using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using PMS.Application.Services;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Test.Services;
public class CsvServiceTest
{     
    private string GetProductCsvTestFile(string fileName)
    {        
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestResources", fileName);
        return basePath;
    }
    
    [Fact]
    public void GetProductCsv_ValidCsvFile_ReturnsListOfProductDtos()
    {
            // Arrange
            var csvService = new CsvService();
            var filePath = GetProductCsvTestFile("valid_product_file_sample.csv");

            // Act
            List<ProductDto> result = csvService.GetProductCsv(filePath);

            // Assert
            Assert.NotNull(result);            

            // Validate the first product
            var firstProduct = result[0];
            Assert.Equal(1, firstProduct.Id);
            Assert.Equal("LC01-76-1038-1", firstProduct.Sku);
            Assert.Equal("EAN090909", firstProduct.Ean);
            Assert.Equal("iPhone 12 / 12 Pro cover - Black", firstProduct.Name);
            Assert.Equal("A black iPhone 12 / 12 Pro cover", firstProduct.Description);
            Assert.Equal("Black", firstProduct.Color);
            Assert.Equal("Silicone / TPU", firstProduct.Material);
            Assert.Equal("Cover", firstProduct.ProductType);
            Assert.Equal("Smartphone", firstProduct.ProductGroup);
            Assert.Equal("TVC", firstProduct.Supplier);
            Assert.Equal("101123911A", firstProduct.SupplierSku);
            Assert.Equal(12, firstProduct.TemplateNo);
            Assert.Equal(457, firstProduct.List);
            Assert.Equal(0.1f, firstProduct.Weight);
            Assert.Equal("DKK", firstProduct.Currency);
            Assert.Equal(5.0, firstProduct.Cost);            
            Assert.Equal(99.0f, firstProduct.Price);
            Assert.Equal(79.0f, firstProduct.SpecialPrice);            
    }
}