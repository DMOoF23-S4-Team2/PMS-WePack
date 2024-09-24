using Moq;
using PMS.Application.DTOs.Product;
using PMS.Application.Interfaces;
using PMS.Application.Services;
using PMS.Core.Entities;
using PMS.Core.Repositories;

namespace PMS.Application.Test.Services;

public class ProductServiceTest
{
    private readonly Mock<IProductRepository> _mockProductRepository;
    private readonly IProductService _productService;

    public ProductServiceTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_mockProductRepository.Object);
    }

    [Fact]
    public async Task CreateProduct_ShouldThrowException_WhenProductDtoIsNull()
    {
        // Arrange
        ProductDto productDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _productService.CreateProduct(productDto));
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnProductDto_WhenProductIsCreated()
    {
        // Arrange
        var productDto = new ProductDto { Id = 1, Name = "Test Product" };
        var product = new Product { Id = 1, Name = "Test Product" };
        _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>())).ReturnsAsync(product);

        // Act
        var result = await _productService.CreateProduct(productDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productDto.Id, result.Id);
        Assert.Equal(productDto.Name, result.Name);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test Product" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);

        // Act
        var result = await _productService.GetProduct(productId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productId, result.Id);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Product 1" },
            new Product { Id = 2, Name = "Product 2" }
        };
        _mockProductRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        // Act
        var result = await _productService.GetProducts();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeleteProduct_ShouldCallRepositoryDeleteMethod()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test Product" };
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
        _mockProductRepository.Setup(repo => repo.DeleteAsync(product)).Returns(Task.CompletedTask);

        // Act
        await _productService.DeleteProduct(productId);

        // Assert
        _mockProductRepository.Verify(repo => repo.DeleteAsync(product), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldThrowException_WhenProductDtoIsNull()
    {
        // Arrange
        var productId = 1;
        ProductDto productDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.UpdateProduct(productId, productDto));
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var oldProduct = new Product { Id = productId, Name = "Old Product" };
        var newProductDto = new ProductDto { Id = productId, Name = "New Product" };
        var newProduct = new Product { Id = productId, Name = "New Product" };

        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(oldProduct);
        _mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        // Act
        await _productService.UpdateProduct(productId, newProductDto);

        // Assert
        _mockProductRepository.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Name == newProduct.Name)), Times.Once);
    }

    [Fact]
    public async Task GetProduct_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.GetProduct(productId));
    }

    [Fact]
    public async Task DeleteProduct_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _productService.DeleteProduct(productId));
    }
}