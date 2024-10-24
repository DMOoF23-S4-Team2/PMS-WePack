using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using PMS.Application.Services;
using PMS.Application.DTOs.Product;

namespace PMS.Application.Test.Services;
public class CsvServiceTest
{     
    private List<string> MockedValidCsvFile(){
        // Notice the csv file is with semicolon has delimiter/seperator
        var csvContent = new List<string>
        {
            "id;sku;ean;name;description;color;material;product_type;product_group;supplier;supplier_sku;template_no;list;weight;currency;cost;price;special_price",
            "1;LC01-76-1038-1;EAN090909;iPhone 12 / 12 Pro cover - Black;A black iPhone 12 / 12 Pro cover;Black;Silicone / TPU;Cover;Smartphone;TVC;101123911A;12;457;0.1;DKK;5;99;79",
            "2;LC01-76-1038-2;EAN090909;iPhone 12 / 12 Pro cover – White;A black iPhone 12 / 12 Pro cover;White;PU Leather;Case;Smartphone;TVC;101123911B;11;457;0.1;SEK;5;60;49",
            "3;LC01-76-1038-3;EAN090909;iPhone 12 / 12 Pro cover – Blue;A black iPhone 12 / 12 Pro cover;Blue;Plastic;Cover;Smartphone;TVC;101123911C;132;328;0.9;NOK;5;60;49"
        };
        return csvContent;
    }     
    
    [Fact]
    public void GetProductCsv_ValidCsvFile_ReturnsListOfProductDtos()
    {
        // Arrange
        var csvService = new CsvService();
        // Write the csvContent to a test file
        var filePath = "test-filepath.csv";
        var csvContent = MockedValidCsvFile();
        File.WriteAllLines(filePath, csvContent);

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

        // Clean up (remove the test file)
        File.Delete(filePath);          
    }
}