using System;
using System.IO;
using PMS.Infrastructure.FileHandlers;

namespace PMS.Infrastructure.Test.TestFileHandlers;
public class CsvHandlerTest
{     
    private List<string> MockedValidCsvFile(){
        // Notice the csv file is with semicolon has delimiter/seperator
        var csvContent = new List<string>
        {
            "id;sku;ean;name;description;color;material;product_type;product_group;supplier;supplier_sku;template_no;list;weight;currency;cost;price;special_price;platform",
            "1;LC01-76-1038-1;EAN090909;iPhone 12 / 12 Pro cover - Black;A black iPhone 12 / 12 Pro cover;Black;Silicone / TPU;Cover;Smartphone;TVC;101123911A;12;457;0.1;DKK;5;99;79;shopify",
            "2;LC01-76-1038-2;EAN090909;iPhone 12 / 12 Pro cover - White;A black iPhone 12 / 12 Pro cover;White;PU Leather;Case;Smartphone;TVC;101123911B;11;457;0.1;SEK;5;60;49;shopify",
            "3;LC01-76-1038-3;EAN090909;iPhone 12 / 12 Pro cover - Blue;A black iPhone 12 / 12 Pro cover;Blue;Plastic;Cover;Smartphone;TVC;101123911C;132;328;0.9;NOK;5;60;49;shopify"
        };
        return csvContent;
    } 

    // [Fact]
    // public void GetCsv_ValidCsvFile_ChecksContent()
    // {
    //     // Arrange
    //     var csvHandler = new CsvHandler();        
    //     // Write the csvContent to a test file
    //     var filePath = "test-filepath.csv";
    //     var csvContent = MockedValidCsvFile();
    //     File.WriteAllLines(filePath, csvContent);

    //     // Act
    //     var result = csvHandler.GetCsv(filePath);
    //     // Assert
    //     // There should be 4 rows (Including header)
    //     Assert.Equal(4, result.Count); 
    //     // Test header
    //     Assert.Equal("id;sku;ean;name;description;color;material;product_type;product_group;supplier;supplier_sku;template_no;list;weight;currency;cost;price;special_price", result[0]);
    //     // Test first product
    //     Assert.Equal("1;LC01-76-1038-1;EAN090909;iPhone 12 / 12 Pro cover - Black;A black iPhone 12 / 12 Pro cover;Black;Silicone / TPU;Cover;Smartphone;TVC;101123911A;12;457;0.1;DKK;5;99;79", result[1]);
        
    //     // Clean up (remove the test file)
    //     File.Delete(filePath);
    // }
}