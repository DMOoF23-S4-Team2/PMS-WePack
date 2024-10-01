using PMS.Application.Interfaces;
using PMS.API.Controllers;
using PMS.Application.DTOs.Product;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;


namespace PMS.API.Test;
public class ProductControllerTest
{        
    private Mock<IProductService> _mockProductService;
    private readonly ProductController _controller;

    public ProductControllerTest(){
        _mockProductService = new Mock<IProductService>();
        _controller = new ProductController(_mockProductService.Object);
    }

    [Fact]
    public async Task CreateProduct_ReturnsOkResult_WithCreatedProduct()
    {
        // Arrange
        var productDto = new ProductDto { Id = 1, Name = "Test Product" };
        _mockProductService.Setup(service => service.CreateProduct(It.IsAny<ProductDto>()))
                            .ReturnsAsync(productDto);

        // Act
        var result = await _controller.CreateProduct(productDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(productDto.Id, returnValue.Id);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNoContent_WhenProductDeleted()
    {
        // Arrange
        _mockProductService.Setup(service => service.DeleteProduct(It.IsAny<int>()))
                            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteProduct_ReturnsNotFound_WhenProductNotExists()
    {
        // Arrange
        _mockProductService.Setup(service => service.DeleteProduct(It.IsAny<int>()))
                            .Throws(new ArgumentNullException());

        // Act
        var result = await _controller.DeleteProduct(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetProduct_ReturnsOk_WithProduct()
    {
        // Arrange
        var productDto = new ProductDto { Id = 1, Name = "Test Product" };
        _mockProductService.Setup(service => service.GetProduct(1))
                            .ReturnsAsync(productDto);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsType<ProductDto>(okResult.Value);
        Assert.Equal(productDto.Id, returnValue.Id);
    }

    [Fact]
    public async Task GetProduct_ReturnsNotFound_WhenProductNotFound()
    {
        // Arrange
        _mockProductService.Setup(service => service.GetProduct(It.IsAny<int>()))
                            .ReturnsAsync((ProductDto)null);

        // Act
        var result = await _controller.GetProduct(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetProducts_ReturnsOk_WithProductList()
    {
        // Arrange
        var productList = new List<ProductDto>
        {
            new ProductDto { Id = 1, Name = "Product 1" },
            new ProductDto { Id = 2, Name = "Product 2" }
        };

        _mockProductService.Setup(service => service.GetProducts())
                            .ReturnsAsync(productList);

        // Act
        var result = await _controller.GetProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnValue = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
        Assert.Equal(2, returnValue.Count()); // Use Count() to check the number of products
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNoContent_WhenUpdateSuccessful()
    {
        // Arrange
        var productDto = new ProductDto { Id = 1, Name = "Updated Product" };
        _mockProductService.Setup(service => service.UpdateProduct(1, productDto))
                            .Returns(Task.CompletedTask);

        // Act
        var result = await _controller.UpdateProduct(1, productDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateProduct_ReturnsNotFound_WhenProductNotFound()
    {
        // Arrange
        var productDto = new ProductDto { Id = 1, Name = "Updated Product" };
        _mockProductService.Setup(service => service.UpdateProduct(1, productDto))
                            .Throws(new ArgumentNullException());

        // Act
        var result = await _controller.UpdateProduct(1, productDto);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
    }
}