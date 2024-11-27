using AutoMapper;
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
    private readonly Mock<IMapper> _mockMapper = new Mock<IMapper>();

    public ProductServiceTest()
    {
        _mockProductRepository = new Mock<IProductRepository>();
        _productService = new ProductService(_mockProductRepository.Object);
        _mockMapper = new Mock<IMapper>();

    }

    [Fact]
    public async Task CreateProduct_ShouldThrowException_WhenProductDtoIsNull()
    {
        // Arrange
        ProductWithoutIdDto productDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _productService.CreateProduct(productDto));
    }

    [Fact]
    public async Task CreateProduct_ShouldReturnProductDto_WhenProductIsCreated()
    {
        // Arrange
        var productDto = new ProductWithoutIdDto { Name = "Test Product", Sku = "SKU123", Price = 10, SpecialPrice = 5 };
        var product = new Product { Name = "Test Product", Sku = "SKU123", Price = 10, SpecialPrice = 5 };
        _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>())).ReturnsAsync(product);

        // Act
        var result = await _productService.CreateProduct(productDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(productDto.Sku, result.Sku);
        Assert.Equal(productDto.Name, result.Name);
    }

    [Fact]
    public async Task GetProduct_ShouldReturnProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var product = new Product { Id = productId, Name = "Test Product", Sku = "SKU123", Price = 10, SpecialPrice = 5 };
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
    public async Task DeleteProduct_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var sku = "SKU123";
        var product = new Product { Id = 1, Name = "Test Product", Sku = sku, Price = 10, SpecialPrice = 5 };
        _mockProductRepository.Setup(repo => repo.GetBySkuAsync(sku)).ReturnsAsync(product);
        _mockProductRepository.Setup(repo => repo.DeleteAsync(product)).Returns(Task.CompletedTask);

        // Act
        await _productService.DeleteProduct(sku);

        // Assert
        _mockProductRepository.Verify(repo => repo.DeleteAsync(It.Is<Product>(p => p.Sku == sku)), Times.Once);
    }

    [Fact]
    public async Task UpdateProduct_ShouldThrowException_WhenProductDtoIsNull()
    {
        // Arrange
        var productId = 1;
        ProductDto productDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.UpdateProduct(productId, productDto));
    }

    [Fact]
    public async Task UpdateProduct_ShouldUpdateProduct_WhenProductExists()
    {
        // Arrange
        var productId = 1;
        var productDto = new ProductDto {Id = productId, Name = "Updated Product", Sku = "SKU123", Price = 10, SpecialPrice = 5};
        var oldProduct = new Product { Id = productId, Name = "Old Product", Sku = "SKU123", Price = 10, SpecialPrice = 5 };
        var newProduct = new Product { Id = productId, Name = "Updated Product", Sku = "SKU123", Price = 10, SpecialPrice = 5 };

        var mockProductRepository = new Mock<IProductRepository>();
        mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(oldProduct);
        mockProductRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Product>())).Returns(Task.CompletedTask);

        var productService = new ProductService(mockProductRepository.Object);

        // Act
        await productService.UpdateProduct(productId, productDto);

        // Assert
        mockProductRepository.Verify(repo => repo.UpdateAsync(It.Is<Product>(p => p.Name == newProduct.Name)), Times.Once);
    }

    [Fact]
    public async Task GetProduct_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var productId = 1;
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.GetProduct(productId));
    }

    [Fact]
    public async Task DeleteProduct_ShouldThrowException_WhenProductDoesNotExist()
    {
        // Arrange
        var ProductDto = new ProductDto { Id = 1, Sku = "SKU123"};
        _mockProductRepository.Setup(repo => repo.GetByIdAsync(ProductDto.Id)).ReturnsAsync((Product)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _productService.DeleteProduct(ProductDto.Sku));
    }

    [Fact]
    public async Task AddManyProducts_ShouldAddMultipleProducts()
    {
        // Arrange
        var productsDto = new List<ProductWithoutIdDto>
    {
        new ProductWithoutIdDto { Name = "Product 1", Sku = "SKU1", Price = 10, SpecialPrice = 5 },
        new ProductWithoutIdDto { Name = "Product 2", Sku = "SKU2", Price = 20, SpecialPrice = 10 }
    };
        var products = productsDto.Select(dto => new Product { Name = dto.Name, Sku = dto.Sku, Price = dto.Price, SpecialPrice = dto.SpecialPrice }).ToList();

        // Mock the mapping from ProductWithoutIdDto to Product
        _mockMapper.Setup(m => m.Map<List<Product>>(productsDto)).Returns(products);

        // Mock the repository method
        _mockProductRepository.Setup(repo => repo.AddManyAsync(It.IsAny<IEnumerable<Product>>())).Returns(Task.CompletedTask);

        // Act
        await _productService.AddManyProducts(productsDto);

        // Assert
        _mockProductRepository.Verify(repo => repo.AddManyAsync(It.Is<IEnumerable<Product>>(p => p.Count() == productsDto.Count)), Times.Once);
    }

    [Fact]
    public async Task DeleteManyProducts_ShouldCallRepositoryDeleteManyMethod()
    {
        // Arrange
        var products = new List<Product>
    {
        new Product { Id = 1, Sku = "SKU1", Price = 10, SpecialPrice = 5 },
        new Product { Id = 2, Sku = "SKU2", Price = 20, SpecialPrice = 10 }
    };
        var productsDto = new List<ProductWithoutIdDto>
    {
        new ProductWithoutIdDto { Sku = "SKU1", Price = 10, SpecialPrice = 5 },
        new ProductWithoutIdDto { Sku = "SKU2", Price = 20, SpecialPrice = 10 }
    };

        // Mock the mapping from ProductDto to Product
        _mockMapper.Setup(m => m.Map<List<Product>>(productsDto)).Returns(products);

        // Mock the repository method
        _mockProductRepository.Setup(repo => repo.DeleteManyAsync(It.IsAny<IEnumerable<Product>>())).Returns(Task.CompletedTask);

        // Act
        await _productService.DeleteManyProducts(productsDto);

        // Assert
        _mockProductRepository.Verify(repo => repo.DeleteManyAsync(It.Is<IEnumerable<Product>>(p => p.Count() == productsDto.Count)), Times.Once);
    }


}