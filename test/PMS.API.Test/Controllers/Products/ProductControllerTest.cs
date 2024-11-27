using PMS.Application.Interfaces;
using PMS.API.Controllers;
using PMS.Application.DTOs.Product;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace PMS.API.Test
{
    public class ProductControllerTest
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductController _controller;

        public ProductControllerTest()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductController(_mockProductService.Object);
        }

        

        [Fact]
        public async Task AddManyProducts_ReturnsNoContent()
        {
            // Arrange
            var productDtos = new List<ProductWithoutIdDto>
            {
                new ProductWithoutIdDto { Name = "Product 1", Sku = "12345" },
                new ProductWithoutIdDto { Name = "Product 2", Sku = "67890" }
            };

            _mockProductService.Setup(service => service.AddManyProducts(productDtos)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.AddManyProducts(productDtos);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent()
        {
            // Arrange
            var sku = "12345";
            _mockProductService.Setup(service => service.DeleteProduct(sku)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(sku);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteManyProducts_ReturnsNoContent()
        {
            // Arrange
            var productDtos = new List<ProductWithoutIdDto>
            {
                new ProductWithoutIdDto { Name = "Product 1", Sku = "12345" },
                new ProductWithoutIdDto { Name = "Product 2", Sku = "67890" }
            };

            _mockProductService.Setup(service => service.DeleteManyProducts(productDtos)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteManyProducts(productDtos);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task GetProduct_ReturnsOkResult_WithProduct()
        {
            // Arrange
            var id = 1;
            var product = new ProductDto { Id = id, Name = "Test Product", Sku = "12345" };
            _mockProductService.Setup(service => service.GetProduct(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProduct(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ProductDto>(okResult.Value);
            Assert.Equal(product.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetProducts_ReturnsOkResult_WithProducts()
        {
            // Arrange
            var products = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Sku = "12345" },
                new ProductDto { Id = 2, Name = "Product 2", Sku = "67890" }
            };

            _mockProductService.Setup(service => service.GetProducts()).ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<ProductDto>>(okResult.Value);
            Assert.Equal(products.Count, returnValue.Count);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent()
        {
            // Arrange
            var id = 1;
            var productDto = new ProductDto { Id = id, Name = "Updated Product", Sku = "12345" };
            _mockProductService.Setup(service => service.UpdateProduct(id, productDto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct(id, productDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateManyProducts_ReturnsNoContent()
        {
            // Arrange
            var productDtos = new List<ProductDto>
            {
                new ProductDto { Id = 1, Name = "Product 1", Sku = "12345" },
                new ProductDto { Id = 2, Name = "Product 2", Sku = "67890" }
            };

            _mockProductService.Setup(service => service.UpdateManyProducts(productDtos)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateManyProducts(productDtos);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}