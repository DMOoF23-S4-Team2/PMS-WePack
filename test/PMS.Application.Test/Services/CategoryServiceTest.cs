using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;
using PMS.Application.Servises;
using PMS.Core.Repositories;
using Xunit;

namespace PMS.Application.Test.Services
{
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
            await Assert.ThrowsAsync<ArgumentNullException>(() => _categoryService.CreateCategory(categoryDto));
        }

        [Fact]
        public async Task GetCategory_ShouldReturnCategory_WhenCategoryExists()
        {
            // Arrange
            var categoryId = 1;
            var categoryDto = new CategoryDto { Id = categoryId, Name = "Test Category" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(categoryDto);

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
            var categories = new List<CategoryDto>
            {
                new CategoryDto { Id = 1, Name = "Category 1" },
                new CategoryDto { Id = 2, Name = "Category 2" }
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
            var categoryDto = new CategoryDto { Id = categoryId, Name = "Test Category" };
            
            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(categoryDto)).Returns(Task.CompletedTask);

            // Act
            await _categoryService.DeleteCategory(categoryId);

            // Assert
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(categoryDto), Times.Once);
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
    }
}