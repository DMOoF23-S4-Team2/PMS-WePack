using System;
using System.IO;
using PMS.Infrastructure.FileHandlers;

namespace PMS.Infrastructure.Test.TestFileHandlers;
public class CsvHandlerTest
{     

    private string GetProductCsvTestFile(string fileName){        
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), "TestResources", fileName);
        return basePath;               
    }

        [Fact]
        public void GetCsv_ValidCsvFile_ChecksContent()
        {
            // Arrange
            var csvHandler = new CsvHandler();
            // Notice the csv file is with semicolon has delimiter/seperator
            var filePath = GetProductCsvTestFile("valid_product_file_sample.csv");

            // Act
            var result = csvHandler.GetCsv(filePath);
            System.Console.WriteLine(result);
            // Assert
            // There should be 4 rows (Including header)
            Assert.Equal(4, result.Count); 
            // Test header
            Assert.Equal("id;sku;ean;name;description;color;material;product_type;product_group;supplier;supplier_sku;template_no;list;weight;currency;cost;price;special_price", result[0]);
            // Test first product
            Assert.Equal("1;LC01-76-1038-1;EAN090909;iPhone 12 / 12 Pro cover - Black;A black iPhone 12 / 12 Pro cover;Black;Silicone / TPU;Cover;Smartphone;TVC;101123911A;12;457;0.1;DKK;5;99;79", result[1]);
        }
}