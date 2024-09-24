using Moq;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;
using PMS.Application.Servises;
using PMS.Core.Entities;
using PMS.Core.Repositories;

namespace PMS.Application.Test.Services;

public class CategoryServiceTest
{
    private readonly Mock<ICategoryRepository> _mockCategoryRepository;
    private readonly ICategoryService _categoryService;

    public CategoryServiceTest()
    {
        _mockCategoryRepository = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_mockCategoryRepository.Object);
    }

    [Fact]
    public async Task CreateCategory_ShouldThrowException_WhenCategoryDtoIsNull()
    {
        // Arrange
        CategoryDto categoryDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<NullReferenceException>(() => _categoryService.CreateCategory(categoryDto));
    }

    [Fact]
    public async Task CreateCategory_ShouldReturnCategoryDto_WhenCategoryIsCreated()
    {
        // Arrange
        var categoryDto = new CategoryDto { Id = 1, Name = "Test Category" };
        var category = new Category { Id = 1, Name = "Test Category" };
        _mockCategoryRepository.Setup(repo => repo.AddAsync(It.IsAny<Category>())).ReturnsAsync(category);

        // Act
        var result = await _categoryService.CreateCategory(categoryDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryDto.Id, result.Id);
        Assert.Equal(categoryDto.Name, result.Name);
    }

    [Fact]
    public async Task GetCategory_ShouldReturnCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Test Category" };
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

        // Act
        var result = await _categoryService.GetCategory(categoryId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(categoryId, result.Id);
    }

    [Fact]
    public async Task GetCategories_ShouldReturnAllCategories()
    {
        // Arrange
        var categories = new List<Category>
        {
            new Category { Id = 1, Name = "Category 1" },
            new Category { Id = 2, Name = "Category 2" }
        };
        _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

        // Act
        var result = await _categoryService.GetCategories();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task DeleteCategory_ShouldCallRepositoryDeleteMethod()
    {
        // Arrange
        var categoryId = 1;
        var category = new Category { Id = categoryId, Name = "Test Category" };
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
        _mockCategoryRepository.Setup(repo => repo.DeleteAsync(category)).Returns(Task.CompletedTask);

        // Act
        await _categoryService.DeleteCategory(categoryId);

        // Assert
        _mockCategoryRepository.Verify(repo => repo.DeleteAsync(category), Times.Once);
    }

    [Fact]
    public async Task UpdateCategory_ShouldThrowException_WhenCategoryDtoIsNull()
    {
        // Arrange
        var categoryId = 1;
        CategoryDto categoryDto = null;

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _categoryService.UpdateCategory(categoryId, categoryDto));
    }

    [Fact]
    public async Task UpdateCategory_ShouldUpdateCategory_WhenCategoryExists()
    {
        // Arrange
        var categoryId = 1;
        var oldCategory = new Category { Id = categoryId, Name = "Old Category" };
        var newCategoryDto = new CategoryDto { Id = categoryId, Name = "New Category" };
        var newCategory = new Category { Id = categoryId, Name = "New Category" };

        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(oldCategory);
        _mockCategoryRepository.Setup(repo => repo.UpdateAsync(It.IsAny<Category>())).Returns(Task.CompletedTask);

        // Act
        await _categoryService.UpdateCategory(categoryId, newCategoryDto);

        // Assert
        _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.Is<Category>(c => c.Name == newCategory.Name)), Times.Once);
    }

    [Fact]
    public async Task GetCategory_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = 1;
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _categoryService.GetCategory(categoryId));
    }

    [Fact]
    public async Task DeleteCategory_ShouldThrowException_WhenCategoryDoesNotExist()
    {
        // Arrange
        var categoryId = 1;
        _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _categoryService.DeleteCategory(categoryId));
    }
}