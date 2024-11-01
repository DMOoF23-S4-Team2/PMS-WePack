using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using PMS.Application.Services;
using PMS.Application.Interfaces;
using PMS.Application.DTOs.Product;
using PMS.Infrastructure.Interfaces;

namespace PMS.Application.Test.Services;
public class CsvServiceTest
{   
    // Mocked CSV lists
    private List<string> MockedValidCsvFileWithOneProduct(){        
        var csvContent = new List<string>
        {
            "id;sku;ean;name;description;color;material;product_type;product_group;supplier;supplier_sku;template_no;list;weight;currency;cost;price;special_price;platform",
            "1;LC01-76-1038-1;EAN090909;iPhone 12 / 12 Pro cover - Black;A black iPhone 12 / 12 Pro cover;Black;Silicone / TPU;Cover;Smartphone;TVC;101123911A;12;457;0.1;DKK;5;99;79;shopify",
        };
        return csvContent;
    } 

    private List<string> MockedValidCsvFileWithoutID(){
        // Notice the csv file is with semicolon has delimiter/seperator
        var csvContent = new List<string>
        {
            "sku;ean;name;description;color;material;product_type;product_group;supplier;supplier_sku;template_no;list;weight;currency;cost;price;special_price;platform",
            "LC01-76-1038-1;EAN090909;iPhone 12 / 12 Pro cover - Black;A black iPhone 12 / 12 Pro cover;Black;Silicone / TPU;Cover;Smartphone;TVC;101123911A;12;457;0.1;DKK;5;99;79;shopify",
            "LC01-76-1038-2;EAN090909;iPhone 12 / 12 Pro cover - White;A black iPhone 12 / 12 Pro cover;White;PU Leather;Case;Smartphone;TVC;101123911B;11;457;0.1;SEK;5;60;49;shopify",
            "LC01-76-1038-3;EAN090909;iPhone 12 / 12 Pro cover - Blue;A black iPhone 12 / 12 Pro cover;Blue;Plastic;Cover;Smartphone;TVC;101123911C;132;328;0.9;NOK;5;60;49;shopify"
        };
        return csvContent;
    }   

    private List<string> MockedValidCsvFileWithID(){
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
    
    // Tests for DetermineMethod
    [Fact]
    public async Task TestCsvFile_CreateOneProduct()
    {
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithOneProduct());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act
        await csvService.DetermineMethod("test-filepath-create.csv");        
        // Assert
        // Verify that CreateProduct was called once with the expected product
        mockedProductService.Verify(service => service.CreateProduct(It.IsAny<ProductWithoutIdDto>()), Times.Once);      
    }

    [Fact]
    public async Task TestCsvFile_CreateManyProducts()
    {
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithoutID());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act
        await csvService.DetermineMethod("test-filepath-create.csv");        
        
        // Assert        
        mockedProductService.Verify(service => service.AddManyProducts(It.IsAny<IEnumerable<ProductWithoutIdDto>>()), Times.Once);    
    }

    [Fact]
    public async Task TestCsvFile_UpdateOneProduct()
    {
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithOneProduct());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act
        await csvService.DetermineMethod("test-filepath-update.csv");        
        
        // Assert        
        mockedProductService.Verify(service => service.UpdateProduct(It.IsAny<int>(), It.IsAny<ProductDto>()), Times.Once);
    }        

    [Fact]
    public async Task TestCsvFile_UpdateManyProducts()
    {
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithID());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act
        await csvService.DetermineMethod("test-filepath-update.csv");        
        
        // Assert        
        mockedProductService.Verify(service => service.UpdateManyProducts(It.IsAny<IEnumerable<ProductDto>>()), Times.Once);    
    }    

    [Fact]
    public async Task TestCsvFile_DeleteOneProduct()
    {
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithOneProduct());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act
        await csvService.DetermineMethod("test-filepath-delete.csv");        
        // Assert        
        mockedProductService.Verify(service => service.DeleteProduct(1), Times.Once);
    }

    [Fact]
    public async Task TestCsvFile_DeleteManyProducts()
    {
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithID());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act
        await csvService.DetermineMethod("test-filepath-delete.csv");        
        // Assert        
        mockedProductService.Verify(service => service.DeleteManyProducts(It.IsAny<IEnumerable<ProductDto>>()), Times.Once);   
    }    


    // Testing single methods
    [Fact]
    public async void TestCreateProduct(){
        // Arrange        
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();          
        // Mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                        .Returns(MockedValidCsvFileWithOneProduct());          
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);         
        // Act    
        await csvService.CreateProduct("test-filepath.csv");        
        // Assert
        mockedProductService.Verify(service => service.CreateProduct(It.IsAny<ProductWithoutIdDto>()), Times.Once);       
    }

    [Fact]
    public async void TestAddManyProductsFromCsvFile(){
        // Arrange
        var mockedCsvHandler = new Mock<ICsvHandler>();
        var mockedProductService = new Mock<IProductService>();
        // Setup the mocked CSV handler to return mock data
        mockedCsvHandler.Setup(handler => handler.GetCsv(It.IsAny<string>()))
                    .Returns(MockedValidCsvFileWithoutID());
        // Create an instance of CsvService with the mocked dependencies
        var csvService = new CsvService(mockedCsvHandler.Object, mockedProductService.Object);
        // Act
        await csvService.AddManyProducts("test-withoutid_filepath-create.csv");
        // Assert
        mockedProductService.Verify(service => service.AddManyProducts(It.IsAny<IEnumerable<ProductWithoutIdDto>>()), Times.Once);        
    }


}