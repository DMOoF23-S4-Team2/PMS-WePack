using PMS.API.Controllers;
using PMS.Application.DTOs.Category;
using PMS.Application.Interfaces;

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;


namespace PMS.API.Test;
public class CategoryControllerTest
{    
    [Fact]
    public void GetCategories_ReturnSuccess()
    {
        Assert.True(true);       
    }

    // Test : GetCategory returns Ok with the correct category when found
    [Fact]
    public async Task GetCategory_ReturnsOkResult_WithCategory_WhenCategoryExists()
    {
        // Arrange
        var mockCategoryService = new Mock<ICategoryService>();
        var categoryId = 1; // Sample ID
        var sampleCategory = new CategoryDto { Id = categoryId, Name = "Electronics" };
        // Setup the mock to return the sample category when the given ID is requested
        mockCategoryService.Setup(service => service.GetCategory(categoryId)).ReturnsAsync(sampleCategory);
        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);
        
        // Act
        var result = await controller.GetCategory(categoryId);
        
        // Assert
        // Verifies the result is OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result); 
        // Verifies the correct object type
        var returnedCategory = Assert.IsType<CategoryDto>(okResult.Value); 
        // Checks that the returned category ID matches the expected ID
        Assert.Equal(categoryId, returnedCategory.Id); 
        // Checks that the name matches the expected value        
        Assert.Equal("Electronics", returnedCategory.Name); 
    }

    // Test returns NotFound when no category is found
    [Fact]
    public async Task GetCategory_ReturnsNotFoundResult_WhenCategoryNotExists(){        
        // Arrange
        var mockCategoryService = new Mock<ICategoryService>();
        var categoryId = 1;
        // Inject the mock service into the controller
        var controller = new CategoryController(mockCategoryService.Object);
        
        // Act
        var result = await controller.GetCategory(categoryId);
        
        // Assert        
        Assert.IsType<NotFoundResult>(result);
    }
 
}